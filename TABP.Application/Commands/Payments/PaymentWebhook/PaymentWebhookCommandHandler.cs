using MediatR;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using TABP.Application.Queries.Invoices;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Payments.PaymentWebhook;

public class PaymentWebhookCommandHandler : IRequestHandler<PaymentWebhookCommand>
{
    private readonly IBookingDetailRepository _bookingDetailRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentWebhookCommandHandler(IBookingDetailRepository bookingDetailRepository,
        IBookingRepository bookingRepository, IPaymentRepository paymentRepository,
        IInvoiceRepository invoiceRepository, IRoomRepository roomRepository, IConfiguration configuration,
        IEmailService emailService, IUnitOfWork unitOfWork)
    {
        _bookingDetailRepository = bookingDetailRepository;
        _bookingRepository = bookingRepository;
        _paymentRepository = paymentRepository;
        _invoiceRepository = invoiceRepository;
        _roomRepository = roomRepository;
        _configuration = configuration;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PaymentWebhookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                request.DataStream,
                request.Signature,
                _configuration["StripeWebhookSecret"]
            );

            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;
                var bookingId = new Guid(session.Metadata["booking_id"]);
                var userId = new Guid(session.Metadata["user_id"]);
                var email = session.Metadata["user_email"];

                var booking = await _bookingRepository.GetByIdAsync(bookingId) ??
                              throw new NotFoundException($"Booking with id {bookingId} wasn't found.");

                if (booking.UserId != userId)
                {
                    throw new UnauthorizedAccessException("You are not allowed to checkout this booking.");
                }

                var bookingDetails = await _bookingDetailRepository
                    .GetByBookingIdAsync(bookingId);

                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    var today = DateOnly.FromDateTime(DateTime.Today);
                    var tomorrow = today.AddDays(1);

                    if (booking.CheckInDate == today || booking.CheckInDate == tomorrow)
                    {
                        foreach (var bookingDetail in bookingDetails)
                        {
                            await _roomRepository.UpdateStatusToReservedByIdAsync(bookingDetail.RoomId);
                        }
                    }

                    var payment = new Payment
                    {
                        Id = new Guid(),
                        UserId = userId,
                        BookingId = booking.Id,
                        PaymentDate = DateTime.Now,
                        PaymentStatus = PaymentStatus.Succeeded,
                        TotalPrice = booking.TotalPrice
                    };

                    await _paymentRepository.CreateAsync(payment);

                    booking.PaymentStatus = PaymentStatus.Succeeded;
                    booking.BookingStatus = BookingStatus.Confirmed;

                    await _bookingRepository.UpdateAsync(booking);

                    await _unitOfWork.SaveChangesAsync();

                    var invoice = new DAL.Entities.Invoice
                    {
                        Id = new Guid(),
                        BookingId = booking.Id,
                        TotalPrice = booking.TotalPrice,
                        InvoiceDate = DateTime.Now,
                        PaymentStatus = payment.PaymentStatus
                    };

                    await _invoiceRepository.CreateAsync(invoice);

                    await _unitOfWork.SaveChangesAsync();

                    await _emailService.SendEmailAsync(email, "Booking Invoice", new EmailInvoiceBody
                    {
                        InvoiceId = invoice.Id,
                        BookingId = invoice.BookingId,
                        PaymentMethod = booking.PaymentMethod.ToString(),
                        PaymentStatus = payment.PaymentStatus.ToString(),
                        PaymentDate = payment.PaymentDate,
                        TotalAmount = invoice.TotalPrice,
                        InvoiceDate = invoice.InvoiceDate
                    });

                    await _unitOfWork.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new InvalidOperationException("An error occurred while processing the payment.", ex);
                }
            }
        }
        catch (StripeException e)
        {
            throw new ArgumentException("Stripe error occurred: " + e.Message, e);
        }
        catch (Exception e)
        {
            throw new ApplicationException("An unexpected error occurred.", e);
        }
    }
}