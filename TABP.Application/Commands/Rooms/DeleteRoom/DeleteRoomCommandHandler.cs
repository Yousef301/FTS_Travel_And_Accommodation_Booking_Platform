using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Rooms.DeleteRoom;

public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoomCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork,
        IHotelRepository hotelRepository)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _hotelRepository = hotelRepository;
    }

    public async Task Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        if (!await _hotelRepository.ExistsAsync(r => r.Id == request.HotelId))
        {
            throw new NotFoundException($"Hotel with id {request.Id} wasn't found");
        }

        var room = await _roomRepository.GetByIdAsync(request.Id, request.HotelId) ??
                   throw new NotFoundException($"Room with id {request.Id} wasn't found");

        _roomRepository.Delete(room);

        await _unitOfWork.SaveChangesAsync();
    }
}