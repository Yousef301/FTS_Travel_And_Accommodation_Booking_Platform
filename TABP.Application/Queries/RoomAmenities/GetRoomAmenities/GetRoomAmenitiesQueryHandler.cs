using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.RoomAmenities.GetRoomAmenities;

public class GetRoomAmenitiesQueryHandler : IRequestHandler<GetRoomAmenitiesQuery, IEnumerable<RoomAmenityResponse>>
{
    private readonly IRoomAmenityRepository _roomAmenityRepository;
    private readonly IMapper _mapper;

    public GetRoomAmenitiesQueryHandler(IRoomAmenityRepository roomAmenityRepository, IMapper mapper)
    {
        _roomAmenityRepository = roomAmenityRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoomAmenityResponse>> Handle(GetRoomAmenitiesQuery request,
        CancellationToken cancellationToken)
    {
        var roomAmenities = await _roomAmenityRepository
            .GetRoomAmenitiesAsync(request.RoomId);

        return _mapper.Map<IEnumerable<RoomAmenityResponse>>(roomAmenities);
    }
}