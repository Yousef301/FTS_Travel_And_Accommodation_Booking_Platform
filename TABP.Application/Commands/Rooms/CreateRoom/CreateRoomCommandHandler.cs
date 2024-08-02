using AutoMapper;
using MediatR;
using TABP.Application.Queries.Rooms;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.Rooms.CreateRoom;

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomResponse>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateRoomCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<RoomResponse> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = _mapper.Map<Room>(request);

        room.Id = new Guid();

        await _roomRepository.CreateAsync(room);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<RoomResponse>(room);
    }
}