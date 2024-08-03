using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Images.Hotels.GetHotelImages;

public class GetHotelImagesQueryHandler : IRequestHandler<GetHotelImagesQuery, IEnumerable<string>>
{
    private readonly IImageRepository<HotelImage> _hotelImageRepository;
    private readonly IImageService _imageService;

    public GetHotelImagesQueryHandler(IImageRepository<HotelImage> hotelImageRepository, IImageService imageService)
    {
        _hotelImageRepository = hotelImageRepository;
        _imageService = imageService;
    }

    public async Task<IEnumerable<string>> Handle(GetHotelImagesQuery request, CancellationToken cancellationToken)
    {
        var hotelImages = await _hotelImageRepository.GetImagesPathAsync(request.HotelId);
        var images = await _imageService.GetSpecificImagesAsync(hotelImages);

        return images;
    }
}