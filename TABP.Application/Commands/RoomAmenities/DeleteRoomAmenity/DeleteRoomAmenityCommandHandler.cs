using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.RoomAmenities.DeleteRoomAmenity;

public class DeleteRoomAmenityCommandHandler : IRequestHandler<DeleteRoomAmenityCommand>
{
    private readonly IRoomAmenityRepository _roomAmenityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoomAmenityCommandHandler(IRoomAmenityRepository roomAmenityRepository,
        IUnitOfWork unitOfWork)
    {
        _roomAmenityRepository = roomAmenityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteRoomAmenityCommand request, CancellationToken cancellationToken)
    {
        await _roomAmenityRepository.DeleteAsync(request.Id);

        await _unitOfWork.SaveChangesAsync();
    }
}