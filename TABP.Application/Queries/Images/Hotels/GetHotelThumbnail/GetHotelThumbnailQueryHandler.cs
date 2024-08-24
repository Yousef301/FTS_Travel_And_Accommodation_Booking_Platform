using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Queries.Images.Hotels.GetHotelThumbnail;

public class GetHotelThumbnailQueryHandler : IRequestHandler<GetHotelThumbnailQuery, ImageResponse>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IImageService _imageService;

    public GetHotelThumbnailQueryHandler(IHotelRepository hotelRepository,
        IImageService imageService)
    {
        _hotelRepository = hotelRepository;
        _imageService = imageService;
    }

    public async Task<ImageResponse> Handle(GetHotelThumbnailQuery request,
        CancellationToken cancellationToken)
    {
        var thumbnailPath = await _hotelRepository.GetThumbnailPathAsync(request.HotelId) ??
                            throw new NotFoundException($"Thumbnail for hotel", request.HotelId);

        var image = await _imageService.GetImageAsync(thumbnailPath) ??
                    throw new NotFoundException($"Thumbnail for hotel", request.HotelId);

        return new ImageResponse
        {
            Image = image,
            Extension = Path.GetExtension(thumbnailPath).TrimStart('.')
        };
    }
}