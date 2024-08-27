using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Shared.Exceptions;

namespace TABP.Application.Queries.Images.Hotels.GetHotelImageById;

public class GetHotelImageByIdQueryHandler : IRequestHandler<GetHotelImageByIdQuery, ImageResponse>
{
    private readonly IImageRepository<HotelImage> _hotelImageRepository;
    private readonly IImageService _imageService;

    public GetHotelImageByIdQueryHandler(IImageRepository<HotelImage> hotelImageRepository,
        IImageService imageService)
    {
        _hotelImageRepository = hotelImageRepository;
        _imageService = imageService;
    }

    public async Task<ImageResponse> Handle(GetHotelImageByIdQuery request,
        CancellationToken cancellationToken)
    {
        var hotelImage = await _hotelImageRepository.GetByIdAsync(hi => hi.Id == request.ImageId) ??
                         throw new NotFoundException($"Hotel Image", request.ImageId);

        var image = await _imageService.GetImageAsync(hotelImage.ImagePath) ??
                    throw new NotFoundException($"Hotel Image", request.ImageId);

        return new ImageResponse
        {
            Image = image,
            Extension = Path.GetExtension(hotelImage.ImagePath).TrimStart('.')
        };
    }
}