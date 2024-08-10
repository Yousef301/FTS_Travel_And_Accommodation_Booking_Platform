using MediatR;

namespace TABP.Application.Queries.Hotels.GetRecentlyVisitedHotels;

public class GetRecentlyVisitedHotelsQuery : IRequest<IEnumerable<RecentlyVisitedHotelsResponse>>
{
    public Guid UserId { get; set; }
    public int Count { get; set; }
}