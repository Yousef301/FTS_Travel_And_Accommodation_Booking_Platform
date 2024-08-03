using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Images.Hotels.GetHotelThumbnail;

public class GetHotelThumbnailQueryHandler : IRequestHandler<GetHotelThumbnailQuery, ImageResponse>
{
    private readonly IHotelImageRepository _hotelImageRepository;
    private readonly IImageService _imageService;

    public GetHotelThumbnailQueryHandler(IHotelImageRepository hotelImageRepository, IImageService imageService)
    {
        _hotelImageRepository = hotelImageRepository;
        _imageService = imageService;
    }

    public async Task<ImageResponse> Handle(GetHotelThumbnailQuery request, CancellationToken cancellationToken)
    {
        var thumbnailPath = await _hotelImageRepository.GetThumbnailPathAsync(request.HotelId);
        var image = await _imageService.GetImageAsync(thumbnailPath);

        return new ImageResponse
        {
            Image = image,
            Extension = Path.GetExtension(thumbnailPath).TrimStart('.')
        };
    }
}