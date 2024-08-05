using MediatR;

namespace TABP.Application.Commands.Bookings.CheckoutBooking;

public class CheckoutBookingCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid BookingId { get; set; }
}