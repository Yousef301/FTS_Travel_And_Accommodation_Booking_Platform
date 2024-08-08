using MediatR;

namespace TABP.Application.Commands.Bookings.CheckoutBooking;

public class CheckoutBookingCommand : IRequest<string>
{
    public Guid UserId { get; set; }
    public Guid BookingId { get; set; }
    public string UserEmail { get; set; }
}