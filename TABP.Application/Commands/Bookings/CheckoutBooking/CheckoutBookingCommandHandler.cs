using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;
using TABP.Domain.Models;
using TABP.Domain.Services.Interfaces;

namespace TABP.Application.Commands.Bookings.CheckoutBooking;

public class CheckoutBookingCommandHandler : IRequestHandler<CheckoutBookingCommand, string>
{
    private readonly ISecretsManagerService _secretsManagerService;
    private readonly IBookingRepository _bookingRepository;
    private readonly IPaymentService _paymentService;

    public CheckoutBookingCommandHandler(IBookingRepository bookingRepository,
        IPaymentService paymentService,
        ISecretsManagerService secretsManagerService)
    {
        _bookingRepository = bookingRepository;
        _secretsManagerService = secretsManagerService;
        _paymentService = paymentService;
    }

    public async Task<string> Handle(CheckoutBookingCommand request,
        CancellationToken cancellationToken)
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

        var paymentUrls = _secretsManagerService.GetSecretAsDictionaryAsync("dev_fts_payment").Result ??
                          throw new ArgumentNullException(nameof(_secretsManagerService));

        var successUrl = paymentUrls["SuccessUrl"] + $"{booking.Id}/invoice";
        var cancelUrl = paymentUrls["CancelUrl"] + $"{booking.Id}";

        var paymentData = new PaymentData
        {
            TotalPrice = booking.TotalPrice,
            Currency = "usd",
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            BookingId = booking.Id,
            UserId = request.UserId,
            UserEmail = request.UserEmail
        };

        var checkoutUrl =
            await _paymentService.CreateCheckoutSessionAsync(paymentData);


        return checkoutUrl;
    }
}