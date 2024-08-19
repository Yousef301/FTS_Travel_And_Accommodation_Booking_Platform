using MediatR;
using TABP.Application.Queries.Bookings;

namespace TABP.Application.Commands.Bookings.CancelBooking;

public class CancelBookingCommand : IRequest<CancelBookingResponse>
{
    public Guid UserId { get; set; }
    public Guid BookingId { get; set; }
}