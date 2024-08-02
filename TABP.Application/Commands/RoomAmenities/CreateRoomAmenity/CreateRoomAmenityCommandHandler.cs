using AutoMapper;
using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.RoomAmenities.CreateRoomAmenity;

public class CreateRoomAmenityCommandHandler : IRequestHandler<CreateRoomAmenityCommand>
{
    private readonly IRoomAmenityRepository _roomAmenityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateRoomAmenityCommandHandler(IRoomAmenityRepository roomAmenityRepository, IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _roomAmenityRepository = roomAmenityRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(CreateRoomAmenityCommand request, CancellationToken cancellationToken)
    {
        var roomAmenity = _mapper.Map<RoomAmenity>(request);

        await _roomAmenityRepository.CreateAsync(roomAmenity);

        await _unitOfWork.SaveChangesAsync();
    }
}