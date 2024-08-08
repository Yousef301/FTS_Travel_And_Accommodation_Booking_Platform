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


        var successUrl = _configuration["Payment:SuccessUrl"] + $"{booking.Id}/invoice";
        var cancelUrl = _configuration["Payment:CancelUrl"] + $"{booking.Id}";

        var checkoutUrl =
            await _paymentService.CreateCheckoutSessionAsync(Convert.ToDecimal(booking.TotalPrice),
                "usd", successUrl, cancelUrl, booking.Id.ToString(), request.UserId.ToString(),
                request.UserEmail);


        return checkoutUrl;
    }
}