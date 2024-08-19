using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Images.Rooms.CreateRoomImage;

public class CreateRoomImageCommandHandler : IRequestHandler<CreateRoomImageCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly IRoomRepository _roomRepository;
    private readonly IImageRepository<RoomImage> _roomImageRepository;

    public CreateRoomImageCommandHandler(IUnitOfWork unitOfWork,
        IImageService imageService,
        IRoomRepository roomRepository,
        IImageRepository<RoomImage> roomImageRepository)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _roomRepository = roomRepository;
        _roomImageRepository = roomImageRepository;
    }


    public async Task Handle(CreateRoomImageCommand request,
        CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(request.RoomId, request.HotelId) ??
                   throw new NotFoundException(nameof(Room), request.RoomId);

        room.RoomNumber = room.RoomNumber.ToLower();

        await _imageService.UploadImagesAsync(request.Images, new Dictionary<string, object>
        {
            { "folder", "rooms" },
            { "prefix", room.RoomNumber },
        });

        var roomImages = request.Images.Select(image => new RoomImage
        {
            Id = new Guid(),
            RoomId = request.RoomId,
            ImagePath = $"rooms/{room.RoomNumber}_{image.FileName}".Replace(' ', '_'),
            Thumbnail = false
        });

        await _roomImageRepository.AddRangeAsync(roomImages);
        await _unitOfWork.SaveChangesAsync();
    }
}