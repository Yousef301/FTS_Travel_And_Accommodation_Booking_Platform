using AutoMapper;
using MediatR;
using TABP.Application.Queries.Rooms;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.Rooms.CreateRoom;

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomResponse>
{
    private readonly IRoomAmenityRepository _roomAmenityRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateRoomCommandHandler(IRoomAmenityRepository roomAmenityRepository, IRoomRepository roomRepository,
        IUnitOfWork unitOfWork, IMapper mapper)
    {
        _roomAmenityRepository = roomAmenityRepository;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<RoomResponse> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = _mapper.Map<Room>(request);

        room.Id = new Guid();

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _roomRepository.CreateAsync(room);

            await _unitOfWork.SaveChangesAsync();

            foreach (var amenityId in request.AmenityIds)
            {
                var roomAmenity = new RoomAmenity
                {
                    Id = new Guid(),
                    RoomId = room.Id,
                    AmenityId = amenityId
                };

                await _roomAmenityRepository.CreateAsync(roomAmenity);

                await _unitOfWork.SaveChangesAsync();

                room.RoomAmenities.Add(roomAmenity);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

        return _mapper.Map<RoomResponse>(room);
    }
}