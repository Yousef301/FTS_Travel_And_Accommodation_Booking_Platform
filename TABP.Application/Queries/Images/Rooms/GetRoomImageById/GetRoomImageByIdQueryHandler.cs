using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

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
        var roomImage = await _roomImageRepository.GetByIdAsync(ri => ri.Id == request.ImageId) ??
                        throw new NotFoundException($"Room image with id {request.ImageId} not found.");

        var image = await _imageService.GetImageAsync(roomImage.ImagePath) ??
                    throw new NotFoundException($"Room image with id {request.ImageId} not found.");

        return new ImageResponse
        {
            Image = image,
            Extension = Path.GetExtension(roomImage.ImagePath).TrimStart('.')
        };
    }
}