using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Queries.Cities.GetCitiesForAdmin;

public class GetCitiesForAdminQueryHandler : IRequestHandler<GetCitiesForAdminQuery, PagedList<CityAdminResponse>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    public GetCitiesForAdminQueryHandler(ICityRepository cityRepository, IMapper mapper, IImageService imageService)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
        _imageService = imageService;
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
        }, includeHotels: true, true);

        var mappedCities = _mapper.Map<PagedList<CityAdminResponse>>(cities);

        var thumbnailPaths = mappedCities.Items
            .Where(city => city.ThumbnailUrl != null)
            .Select(city => city.ThumbnailUrl)
            .Distinct()
            .ToList();

        if (thumbnailPaths.Count == 0) return mappedCities;

        var imageUrlsObject = await _imageService.GetImagesUrlsAsync<List<string>>(thumbnailPaths);

        if (imageUrlsObject is List<string> imageUrls)
        {
            MapThumbnailUrls(mappedCities.Items, imageUrls);
        }

        return mappedCities;
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

    private void MapThumbnailUrls(List<CityAdminResponse> cities, List<string> imageUrls)
    {
        foreach (var city in cities)
        {
            if (city.ThumbnailUrl.IsNullOrEmpty()) continue;
            var matchingUrl = imageUrls.FirstOrDefault(url => url.Contains(city.ThumbnailUrl));

            if (matchingUrl != null)
            {
                city.ThumbnailUrl = matchingUrl;
            }
        }
    }
}