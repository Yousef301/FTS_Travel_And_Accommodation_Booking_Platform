using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Images.Hotels.GetHotelImageById;

public class GetHotelImageByIdQueryHandler : IRequestHandler<GetHotelImageByIdQuery, ImageResponse>
{
    private readonly IHotelImageRepository _hotelImageRepository;
    private readonly IImageService _imageService;

    public GetHotelImageByIdQueryHandler(IHotelImageRepository hotelImageRepository, IImageService imageService)
    {
        _hotelImageRepository = hotelImageRepository;
        _imageService = imageService;
    }

    public async Task<ImageResponse> Handle(GetHotelImageByIdQuery request, CancellationToken cancellationToken)
    {
        var hotelImage = await _hotelImageRepository.GetByIdAsync(request.ImageId);
        var image = await _imageService.GetImageAsync(hotelImage.ImagePath);

        return new ImageResponse
        {
            Image = image,
            Extension = Path.GetExtension(hotelImage.ImagePath).TrimStart('.')
        };
    }
}