using MediatR;

namespace TABP.Application.Queries.Bookings.GetBookings;

public class GetBookingsQuery : IRequest<IEnumerable<BookingResponse>>
{
    public Guid UserId { get; set; }
}