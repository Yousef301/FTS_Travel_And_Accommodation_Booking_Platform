using AutoMapper;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.DAL.Models.Procedures;

namespace TABP.Application.Queries.Cities.GetTrendingCities;

public class
    GetTrendingCitiesQueryHandler : IRequestHandler<GetTrendingCitiesQuery, IEnumerable<TrendingCitiesResponse>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    public GetTrendingCitiesQueryHandler(ICityRepository cityRepository, IMapper mapper, IImageService imageService)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
        _imageService = imageService;
    }

    public async Task<IEnumerable<TrendingCitiesResponse>> Handle(GetTrendingCitiesQuery request,
        CancellationToken cancellationToken)
    {
        var trendingCities = await _cityRepository.GetTrendingDestinations();

        var trendingCitiesList = trendingCities.ToList();

        var thumbnailPaths = trendingCitiesList
            .Where(tc => tc.ThumbnailUrl != null)
            .Select(tc => tc.ThumbnailUrl)
            .Distinct()
            .ToList();

        if (thumbnailPaths.Count == 0) return _mapper.Map<IEnumerable<TrendingCitiesResponse>>(trendingCitiesList);

        var imageUrlsObject = await _imageService.GetImagesUrlsAsync<List<string>>(thumbnailPaths);

        if (imageUrlsObject is List<string> imageUrls)
        {
            MapThumbnailUrls(trendingCitiesList, imageUrls);
        }

        return _mapper.Map<IEnumerable<TrendingCitiesResponse>>(trendingCitiesList);
    }

    private void MapThumbnailUrls(List<TrendingCities> cities, List<string> imageUrls)
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