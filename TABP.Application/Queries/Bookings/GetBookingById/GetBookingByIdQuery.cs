using MediatR;

namespace TABP.Application.Queries.Bookings.GetBookingById;

public class GetBookingByIdQuery : IRequest<BookingResponse>
{
    public Guid UserId { get; set; }
    public Guid BookingId { get; set; }
}