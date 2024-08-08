using MediatR;
using TABP.Application.Queries.Invoices;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Enums;

namespace TABP.Application.Commands.Bookings.CheckoutBooking;

public class CheckoutBookingCommandHandler : IRequestHandler<CheckoutBookingCommand>
{
    private readonly IBookingDetailRepository _bookingDetailRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutBookingCommandHandler(IBookingRepository bookingRepository, IPaymentRepository paymentRepository,
        IInvoiceRepository invoiceRepository, IRoomRepository roomRepository, IUnitOfWork unitOfWork,
        IBookingDetailRepository bookingDetailRepository, IEmailService emailService)
    {
        _bookingRepository = bookingRepository;
        _paymentRepository = paymentRepository;
        _invoiceRepository = invoiceRepository;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _bookingDetailRepository = bookingDetailRepository;
        _emailService = emailService;
    }

    public async Task Handle(CheckoutBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId);

        if (booking is not null && booking.BookingStatus != BookingStatus.Pending)
        {
            throw new ArgumentException(
                "Booking is already confirmed or canceled.");
        }

        var bookingDetails = await _bookingDetailRepository.GetByBookingIdAsync(request.BookingId);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // foreach (var bookingDetail in bookingDetails)
            // {
            //     // TODO: If the reservation in the future don't set it to reserved
            //
            //     await _roomRepository.UpdateStatusToReservedByIdAsync(bookingDetail.RoomId);
            // }

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
    }
}