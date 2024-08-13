using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

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
        var room = await _roomRepository.GetByIdAsync(request.Id, request.HotelId) ??
                   throw new NotFoundException($"Room with id {request.Id} wasn't found");

        var updatedRoomDto = _mapper.Map<RoomUpdate>(room);

        request.RoomDocument.ApplyTo(updatedRoomDto);

        _mapper.Map(updatedRoomDto, room);

        _roomRepository.Update(room);

        await _unitOfWork.SaveChangesAsync();
    }
}