using MediatR;

namespace TABP.Application.Queries.Cities.GetTrendingCities;

public class GetTrendingCitiesQuery : IRequest<IEnumerable<TrendingCitiesResponse>>
{
    
}