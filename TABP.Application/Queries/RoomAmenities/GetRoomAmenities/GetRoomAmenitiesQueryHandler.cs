using AutoMapper;
using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Queries.RoomAmenities.GetRoomAmenities;

public class GetRoomAmenitiesQueryHandler : IRequestHandler<GetRoomAmenitiesQuery, IEnumerable<RoomAmenityResponse>>
{
    private readonly IRoomAmenityRepository _roomAmenityRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public GetRoomAmenitiesQueryHandler(IRoomAmenityRepository roomAmenityRepository,
        IRoomRepository roomRepository,
        IMapper mapper)
    {
        _roomAmenityRepository = roomAmenityRepository;
        _mapper = mapper;
        _roomRepository = roomRepository;
    }

    public async Task<IEnumerable<RoomAmenityResponse>> Handle(GetRoomAmenitiesQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _roomRepository.ExistsAsync(r => r.Id == request.RoomId))
        {
            throw new NotFoundException(nameof(Room), request.RoomId);
        }

        var roomAmenities = await _roomAmenityRepository
            .GetRoomAmenitiesAsync(request.RoomId);

        return _mapper.Map<IEnumerable<RoomAmenityResponse>>(roomAmenities);
    }
}