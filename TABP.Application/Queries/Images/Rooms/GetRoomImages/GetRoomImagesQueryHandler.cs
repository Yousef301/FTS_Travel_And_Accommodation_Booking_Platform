using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Images.Rooms.GetRoomImages;

public class GetRoomImagesQueryHandler : IRequestHandler<GetRoomImagesQuery, IEnumerable<Dictionary<string, string>>>
{
    private readonly IImageRepository<RoomImage> _roomImageRepository;
    private readonly IImageService _imageService;

    public GetRoomImagesQueryHandler(IImageRepository<RoomImage> roomImageRepository,
        IImageService imageService)
    {
        _roomImageRepository = roomImageRepository;
        _imageService = imageService;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> Handle(GetRoomImagesQuery request,
        CancellationToken cancellationToken)
    {
        var roomImages = await _roomImageRepository.GetImagesPathAsync(request.RoomId);

        var imagesObject = await _imageService.GetImagesUrlsAsync<IEnumerable<Dictionary<string, string>>>(roomImages);

        if (imagesObject is IEnumerable<Dictionary<string, string>> images)
        {
            return images;
        }

        return new List<Dictionary<string, string>>();
    }
}