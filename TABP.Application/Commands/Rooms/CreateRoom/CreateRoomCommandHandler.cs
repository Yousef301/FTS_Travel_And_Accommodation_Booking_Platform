using AutoMapper;
using MediatR;
using TABP.Application.Queries.Rooms;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Rooms.CreateRoom;

// TODO: Continue here

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomResponse>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateRoomCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork,
        IMapper mapper, IHotelRepository hotelRepository)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _hotelRepository = hotelRepository;
    }


    public async Task<RoomResponse> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId))
            throw new NotFoundException($"Hotel with id {request.HotelId} wasn't found");

        if (await _roomRepository.ExistsAsync(r => r.RoomNumber == request.RoomNumber && r.HotelId == request.HotelId))
            throw new UniqueConstraintViolationException(
                $"Room with number {request.RoomNumber} already exists in this hotel");

        var room = _mapper.Map<Room>(request);

        room.Id = new Guid();

        await _roomRepository.CreateAsync(room);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<RoomResponse>(room);
    }
}