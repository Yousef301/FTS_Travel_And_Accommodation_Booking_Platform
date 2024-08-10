using MediatR;

namespace TABP.Application.Commands.Bookings.CancelBooking;

public class CancelBookingCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid BookingId { get; set; }
}