using MediatR;

namespace TABP.Application.Commands.Bookings.CancelBooking;

public class CancelBookingCommand : IRequest
{
    public Guid BookingId { get; set; }
}