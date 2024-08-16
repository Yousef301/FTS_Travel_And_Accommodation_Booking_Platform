using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.RoomAmenities.CreateRoomAmenity;

public class CreateRoomAmenityCommandHandler : IRequestHandler<CreateRoomAmenityCommand>
{
    private readonly IRoomAmenityRepository _roomAmenityRepository;
    private readonly IAmenityRepository _amenityRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoomAmenityCommandHandler(IRoomAmenityRepository roomAmenityRepository,
        IUnitOfWork unitOfWork,
        IRoomRepository roomRepository,
        IAmenityRepository amenityRepository)
    {
        _roomAmenityRepository = roomAmenityRepository;
        _unitOfWork = unitOfWork;
        _roomRepository = roomRepository;
        _amenityRepository = amenityRepository;
    }

    public async Task Handle(CreateRoomAmenityCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _roomRepository.ExistsAsync(r => r.Id == request.RoomId))
            throw new NotFoundException($"Room with id {request.RoomId} wasn't found");


        var roomAmenities = new List<RoomAmenity>();

        foreach (var amenity in request.AmenitiesIds)
        {
            if (!await _amenityRepository.ExistsAsync(a => a.Id == amenity))
                throw new NotFoundException($"Amenity with id {amenity} wasn't found");

            var createdRoomAmenity = new RoomAmenity
            {
                Id = new Guid(),
                RoomId = request.RoomId,
                AmenityId = amenity
            };

            roomAmenities.Add(createdRoomAmenity);
        }

        _roomAmenityRepository.AddRange(roomAmenities);

        await _unitOfWork.SaveChangesAsync();
    }
}