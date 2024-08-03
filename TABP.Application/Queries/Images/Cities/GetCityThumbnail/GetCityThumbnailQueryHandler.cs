using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Images.Cities.GetCityThumbnail;

public class GetCityThumbnailQueryHandler : IRequestHandler<GetCityThumbnailQuery, ImageResponse>
{
    private readonly IImageRepository<CityImage> _cityImageRepository;
    private readonly IImageService _imageService;

    public GetCityThumbnailQueryHandler(IImageRepository<CityImage> cityImageRepository, IImageService imageService)
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