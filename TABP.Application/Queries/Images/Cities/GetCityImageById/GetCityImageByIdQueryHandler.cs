using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Images.Cities.GetCityImageById;

public class GetCityImageByIdQueryHandler : IRequestHandler<GetCityImageByIdQuery, ImageResponse>
{
    private readonly IImageRepository<CityImage> _cityImageRepository;
    private readonly IImageService _imageService;

    public GetCityImageByIdQueryHandler(IImageRepository<CityImage> cityImageRepository, IImageService imageService)
    {
        _cityImageRepository = cityImageRepository;
        _imageService = imageService;
    }

    public async Task<ImageResponse> Handle(GetCityImageByIdQuery request, CancellationToken cancellationToken)
    {
        var cityImage = await _cityImageRepository.GetByIdAsync(request.ImageId);
        var image = await _imageService.GetImageAsync(cityImage.ImagePath);

        return new ImageResponse
        {
            Image = image,
            Extension = Path.GetExtension(cityImage.ImagePath).TrimStart('.')
        };
    }
}