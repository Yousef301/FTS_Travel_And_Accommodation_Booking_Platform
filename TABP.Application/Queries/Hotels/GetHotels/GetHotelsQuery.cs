using MediatR;

namespace TABP.Application.Queries.Hotels.GetHotels;

public class GetHotelsQuery : IRequest<IEnumerable<HotelResponse>>
{
}