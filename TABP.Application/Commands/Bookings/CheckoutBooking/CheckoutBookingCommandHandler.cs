using MediatR;
using Microsoft.Extensions.Configuration;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Enums;

namespace TABP.Application.Commands.Bookings.CheckoutBooking;

public class CheckoutBookingCommandHandler : IRequestHandler<CheckoutBookingCommand, string>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IPaymentService _paymentService;
    private readonly IConfiguration _configuration;

    public CheckoutBookingCommandHandler(IBookingRepository bookingRepository,
        IPaymentService paymentService, IConfiguration configuration)
    {
        _bookingRepository = bookingRepository;
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
            await _paymentService.CreateCheckoutSessionAsync(booking.TotalPrice,
                "usd", successUrl, cancelUrl, booking.Id.ToString(), request.UserId.ToString(),
                request.UserEmail);


        return checkoutUrl;
    }
}