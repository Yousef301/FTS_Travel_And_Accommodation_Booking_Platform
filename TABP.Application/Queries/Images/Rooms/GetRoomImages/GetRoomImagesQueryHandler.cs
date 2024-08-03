using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Images.Rooms.GetRoomImages;

public class GetRoomImagesQueryHandler : IRequestHandler<GetRoomImagesQuery, IEnumerable<string>>
{
    private readonly IImageRepository<RoomImage> _roomImageRepository;
    private readonly IImageService _imageService;

    public GetRoomImagesQueryHandler(IImageRepository<RoomImage> roomImageRepository, IImageService imageService)
    {
        _roomImageRepository = roomImageRepository;
        _imageService = imageService;
    }

    public async Task<IEnumerable<string>> Handle(GetRoomImagesQuery request, CancellationToken cancellationToken)
    {
        var hotelImages = await _roomImageRepository.GetImagesPathAsync(request.RoomId);
        var images = await _imageService.GetSpecificImagesAsync(hotelImages);

        return images;
    }
}