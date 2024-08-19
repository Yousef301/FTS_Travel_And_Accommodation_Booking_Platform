using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Queries.Images.Hotels.GetHotelThumbnail;

public class GetHotelThumbnailQueryHandler : IRequestHandler<GetHotelThumbnailQuery, ImageResponse>
{
    private readonly IImageRepository<HotelImage> _hotelImageRepository;
    private readonly IImageService _imageService;

    public GetHotelThumbnailQueryHandler(IImageRepository<HotelImage> hotelImageRepository,
        IImageService imageService)
    {
        _hotelImageRepository = hotelImageRepository;
        _imageService = imageService;
    }

    public async Task<ImageResponse> Handle(GetHotelThumbnailQuery request,
        CancellationToken cancellationToken)
    {
        var thumbnailPath = await _hotelImageRepository.GetThumbnailPathAsync(request.HotelId) ??
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