using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Queries.Cities.GetCitiesForAdmin;

public class GetCitiesForAdminQueryHandler : IRequestHandler<GetCitiesForAdminQuery, PagedList<CityAdminResponse>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public GetCitiesForAdminQueryHandler(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<CityAdminResponse>> Handle(GetCitiesForAdminQuery request,
        CancellationToken cancellationToken)
    {
        var cities = await _cityRepository.GetAsync(new Filters<City>
        {
            FilterExpression = GetCityBasedOnNameOrCountryExpression(request.SearchString),
            SortExpression = GetSortExpression(request.SortBy),
            SortOrder = request.SortOrder,
            Page = request.Page,
            PageSize = request.PageSize
        }, includeHotels: true);

        return _mapper.Map<PagedList<CityAdminResponse>>(cities);
    }

    private Expression<Func<City, bool>> GetCityBasedOnNameOrCountryExpression(string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
        {
            return c => true;
        }

        return c => c.Name.Contains(searchString) || c.Country.Contains(searchString);
    }

    private Expression<Func<City, object>> GetSortExpression(string? sortBy)
    {
        return sortBy?.ToLower() switch
        {
            "country" => c => c.Country,
            _ => c => c.Name
        };
    }
}