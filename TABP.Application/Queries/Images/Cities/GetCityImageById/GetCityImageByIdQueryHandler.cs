using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Shared.Exceptions;

namespace TABP.Application.Queries.Images.Cities.GetCityImageById;

public class GetCityImageByIdQueryHandler : IRequestHandler<GetCityImageByIdQuery, ImageResponse>
{
    private readonly IImageRepository<CityImage> _cityImageRepository;
    private readonly IImageService _imageService;

    public GetCityImageByIdQueryHandler(IImageRepository<CityImage> cityImageRepository,
        IImageService imageService)
    {
        _cityImageRepository = cityImageRepository;
        _imageService = imageService;
    }

    public async Task<ImageResponse> Handle(GetCityImageByIdQuery request,
        CancellationToken cancellationToken)
    {
        var cityImage = await _cityImageRepository.GetByIdAsync(ci => ci.Id == request.ImageId) ??
                        throw new NotFoundException("City Image", request.ImageId);

        var image = await _imageService.GetImageAsync(cityImage.ImagePath) ??
                    throw new NotFoundException("City Image", request.ImageId);

        return new ImageResponse
        {
            Image = image,
            Extension = Path.GetExtension(cityImage.ImagePath).TrimStart('.')
        };
    }
}