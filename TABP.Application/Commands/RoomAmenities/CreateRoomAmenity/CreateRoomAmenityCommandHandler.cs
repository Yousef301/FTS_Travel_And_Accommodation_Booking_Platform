using AutoMapper;
using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.RoomAmenities.CreateRoomAmenity;

public class CreateRoomAmenityCommandHandler : IRequestHandler<CreateRoomAmenityCommand>
{
    private readonly IRoomAmenityRepository _roomAmenityRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateRoomAmenityCommandHandler(IRoomAmenityRepository roomAmenityRepository, IUnitOfWork unitOfWork,
        IMapper mapper, IRoomRepository roomRepository)
    {
        _roomAmenityRepository = roomAmenityRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _roomRepository = roomRepository;
    }

    public async Task Handle(CreateRoomAmenityCommand request, CancellationToken cancellationToken)
    {
        if (!await _roomRepository.ExistsAsync(r => r.Id == request.RoomId))
            throw new NotFoundException($"Room with id {request.RoomId} wasn't found");

        var roomAmenity = _mapper.Map<RoomAmenity>(request);

        await _roomAmenityRepository.CreateAsync(roomAmenity);

        await _unitOfWork.SaveChangesAsync();
    }
}