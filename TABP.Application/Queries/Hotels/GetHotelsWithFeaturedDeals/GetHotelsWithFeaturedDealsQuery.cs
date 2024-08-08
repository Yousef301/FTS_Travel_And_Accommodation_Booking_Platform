using MediatR;

namespace TABP.Application.Queries.Hotels.GetHotelsWithFeaturedDeals;

public class GetHotelsWithFeaturedDealsQuery : IRequest<IEnumerable<HotelWithFeaturedDealResponse>>
{
    public int Count { get; set; }
}