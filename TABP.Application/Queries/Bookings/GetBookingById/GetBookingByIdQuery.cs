using MediatR;

namespace TABP.Application.Queries.Bookings.GetBookingById;

public class GetBookingByIdQuery : IRequest<BookingResponse>
{
    public Guid BookingId { get; set; }
}