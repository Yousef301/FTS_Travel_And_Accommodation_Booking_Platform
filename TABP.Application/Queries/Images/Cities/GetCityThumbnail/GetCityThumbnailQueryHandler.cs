using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Queries.Images.Cities.GetCityThumbnail;

public class GetCityThumbnailQueryHandler : IRequestHandler<GetCityThumbnailQuery, ImageResponse>
{
    private readonly ICityRepository _cityRepository;
    private readonly IImageService _imageService;

    public GetCityThumbnailQueryHandler(ICityRepository cityRepository,
        IImageService imageService)
    {
        _cityRepository = cityRepository;
        _imageService = imageService;
    }

    public async Task<ImageResponse> Handle(GetCityThumbnailQuery request,
        CancellationToken cancellationToken)
    {
        var thumbnailPath = await _cityRepository.GetThumbnailPathAsync(request.CityId) ??
                            throw new NotFoundException($"Thumbnail for city", request.CityId);

        var image = await _imageService.GetImageAsync(thumbnailPath) ??
                    throw new NotFoundException($"Thumbnail for city", request.CityId);

        return new ImageResponse
        {
            Image = image,
            Extension = Path.GetExtension(thumbnailPath).TrimStart('.')
        };
    }
}