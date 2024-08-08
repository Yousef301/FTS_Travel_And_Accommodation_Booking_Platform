using MediatR;
using Microsoft.Extensions.Configuration;
using TABP.Application.Queries.Invoices;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Enums;

namespace TABP.Application.Commands.Bookings.CheckoutBooking;

public class CheckoutBookingCommandHandler : IRequestHandler<CheckoutBookingCommand, string>
{
    private readonly IBookingDetailRepository _bookingDetailRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IPaymentService _paymentService;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutBookingCommandHandler(IBookingRepository bookingRepository,
        IPaymentRepository paymentRepository, IInvoiceRepository invoiceRepository,
        IRoomRepository roomRepository, IUnitOfWork unitOfWork, IPaymentService paymentService,
        IBookingDetailRepository bookingDetailRepository, IEmailService emailService, IConfiguration configuration)
    {
        _bookingRepository = bookingRepository;
        _paymentRepository = paymentRepository;
        _invoiceRepository = invoiceRepository;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _bookingDetailRepository = bookingDetailRepository;
        _emailService = emailService;
        _configuration = configuration;
        _paymentService = paymentService;
    }

    public async Task<string> Handle(CheckoutBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId);

        if (booking is not null && booking.BookingStatus != BookingStatus.Pending)
        {
            throw new ArgumentException(
                "Booking is already confirmed or canceled.");
        }

        var bookingDetails = await _bookingDetailRepository
            .GetByBookingIdAsync(request.BookingId);

        var successUrl = _configuration["Payment:SuccessUrl"] + $"{booking.Id}/invoice";
        var cancelUrl = _configuration["Payment:CancelUrl"] + $"{booking.Id}";

        var checkoutUrl =
            await _paymentService.CreateCheckoutSessionAsync(Convert.ToDecimal(booking.TotalPrice),
                "usd", successUrl, cancelUrl);

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
                UserId = request.UserId,
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

            var invoice = new Invoice
            {
                Id = new Guid(),
                BookingId = booking.Id,
                TotalPrice = booking.TotalPrice,
                InvoiceDate = DateTime.Now,
                PaymentStatus = payment.PaymentStatus
            };

            await _invoiceRepository.CreateAsync(invoice);

            await _unitOfWork.SaveChangesAsync();

            await _emailService.SendEmailAsync(request.UserEmail, "Booking Invoice", new EmailInvoiceBody
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
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

        return checkoutUrl;
    }
}