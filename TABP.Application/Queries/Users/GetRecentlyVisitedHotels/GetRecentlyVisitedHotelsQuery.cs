using MediatR;

namespace TABP.Application.Queries.Users.GetRecentlyVisitedHotels;

public class GetRecentlyVisitedHotelsQuery : IRequest<IEnumerable<RecentlyVisitedHotelsResponse>>
{
    public Guid UserId { get; set; }
}