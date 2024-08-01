using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.Rooms.UpdateRoom;

public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateRoomCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(request.Id);

        if (room == null)
        {
            throw new Exception("Room not found"); // TODO: Create a custom exception
        }

        var updatedRoomDto = _mapper.Map<RoomUpdate>(room);

        request.RoomDocument.ApplyTo(updatedRoomDto);

        _mapper.Map(updatedRoomDto, room);

        await _roomRepository.UpdateAsync(room);

        await _unitOfWork.SaveChangesAsync();
    }
}