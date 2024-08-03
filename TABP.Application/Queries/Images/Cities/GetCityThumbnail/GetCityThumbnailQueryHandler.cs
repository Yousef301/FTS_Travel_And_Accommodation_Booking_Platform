using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Images.Cities.GetCityThumbnail;

public class GetCityThumbnailQueryHandler : IRequestHandler<GetCityThumbnailQuery, ImageResponse>
{
    private readonly ICityImageRepository _cityImageRepository;
    private readonly IImageService _imageService;

    public GetCityThumbnailQueryHandler(ICityImageRepository cityImageRepository, IImageService imageService)
    {
        _cityImageRepository = cityImageRepository;
        _imageService = imageService;
    }

    public async Task<ImageResponse> Handle(GetCityThumbnailQuery request, CancellationToken cancellationToken)
    {
        var thumbnailPath = await _cityImageRepository.GetThumbnailPathAsync(request.CityId);
        var image = await _imageService.GetImageAsync(thumbnailPath);

        return new ImageResponse
        {
            Image = image,
            Extension = Path.GetExtension(thumbnailPath).TrimStart('.')
        };
    }
}