using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Cities.GetTrendingCities;

public class GetTrendingCitiesQueryHandler : IRequestHandler<GetTrendingCitiesQuery, IEnumerable<TrendingCitiesResponse>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public GetTrendingCitiesQueryHandler(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TrendingCitiesResponse>> Handle(GetTrendingCitiesQuery request,
        CancellationToken cancellationToken)
    {
        var trendingCities = await _cityRepository.GetTrendingDestinations();
        return _mapper.Map<IEnumerable<TrendingCitiesResponse>>(trendingCities);
    }
}