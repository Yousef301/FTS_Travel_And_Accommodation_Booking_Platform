using MediatR;
using Microsoft.Extensions.Configuration;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;

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
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId) ??
                      throw new NotFoundException($"Booking with id {request.BookingId} wasn't found");

        if (booking.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException("You are not allowed to checkout this booking");
        }

        if (booking.BookingStatus != BookingStatus.Pending)
        {
            throw new BookingStatusException();
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