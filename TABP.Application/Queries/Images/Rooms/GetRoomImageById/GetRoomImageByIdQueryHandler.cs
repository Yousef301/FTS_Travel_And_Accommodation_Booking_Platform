using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Images.Rooms.GetRoomImageById;

public class GetRoomImageByIdQueryHandler : IRequestHandler<GetRoomImageByIdQuery, ImageResponse>
{
    private readonly IImageRepository<RoomImage> _roomImageRepository;
    private readonly IImageService _imageService;

    public GetRoomImageByIdQueryHandler(IImageRepository<RoomImage> roomImageRepository,
        IImageService imageService)
    {
        _roomImageRepository = roomImageRepository;
        _imageService = imageService;
    }


    public async Task<ImageResponse> Handle(GetRoomImageByIdQuery request, CancellationToken cancellationToken)
    {
        var roomImage = await _roomImageRepository.GetByIdAsync(request.ImageId);
        var image = await _imageService.GetImageAsync(roomImage.ImagePath);

        return new ImageResponse
        {
            Image = image,
            Extension = Path.GetExtension(roomImage.ImagePath).TrimStart('.')
        };
    }
}