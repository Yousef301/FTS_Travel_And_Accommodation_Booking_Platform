using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

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

    public async Task Handle(DeleteRoomAmenityCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _roomAmenityRepository.ExistsAsync(ra => ra.Id == request.Id
                                                            && ra.RoomId == request.RoomId))
            throw new NotFoundException($"Room amenity with id {request.Id} wasn't found");

        await _roomAmenityRepository.DeleteAsync(request.Id);

        await _unitOfWork.SaveChangesAsync();
    }
}