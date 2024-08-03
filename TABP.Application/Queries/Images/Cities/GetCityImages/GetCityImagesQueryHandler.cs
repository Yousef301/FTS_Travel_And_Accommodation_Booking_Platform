using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Images.Cities.GetCityImages;

public class GetCityImagesQueryHandler : IRequestHandler<GetCityImagesQuery, IEnumerable<string>>
{
    private readonly ICityImageRepository _cityImageRepository;
    private readonly IImageService _imageService;

    public GetCityImagesQueryHandler(ICityImageRepository cityImageRepository, IImageService imageService)
    {
        _cityImageRepository = cityImageRepository;
        _imageService = imageService;
    }

    public async Task<IEnumerable<string>> Handle(GetCityImagesQuery request, CancellationToken cancellationToken)
    {
        var cityImages = await _cityImageRepository.GetImagesPathAsync(request.CityId);
        var images = await _imageService.GetSpecificImagesAsync(cityImages);

        return images;
    }
}