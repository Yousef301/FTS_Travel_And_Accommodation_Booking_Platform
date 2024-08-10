using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Images.Cities.GetCityImages;

public class GetCityImagesQueryHandler : IRequestHandler<GetCityImagesQuery, IEnumerable<Dictionary<string, string>>>
{
    private readonly IImageRepository<CityImage> _cityImageRepository;
    private readonly IImageService _imageService;

    public GetCityImagesQueryHandler(IImageRepository<CityImage> cityImageRepository, IImageService imageService)
    {
        _cityImageRepository = cityImageRepository;
        _imageService = imageService;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> Handle(GetCityImagesQuery request,
        CancellationToken cancellationToken)
    {
        var cityImages = await _cityImageRepository.GetImagesPathAsync(request.CityId);

        var imagesObject = await _imageService.GetImagesUrlsAsync<IEnumerable<Dictionary<string, string>>>(cityImages);

        if (imagesObject is IEnumerable<Dictionary<string, string>> images)
        {
            return images;
        }

        return new List<Dictionary<string, string>>();
    }
}