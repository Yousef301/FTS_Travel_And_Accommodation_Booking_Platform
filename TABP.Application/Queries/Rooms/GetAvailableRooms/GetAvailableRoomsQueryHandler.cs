using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Rooms.GetAvailableRooms;

public class GetAvailableRoomsQueryHandler : IRequestHandler<GetAvailableRoomsQuery, IEnumerable<RoomResponse>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public GetAvailableRoomsQueryHandler(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoomResponse>> Handle(GetAvailableRoomsQuery request,
        CancellationToken cancellationToken)
    {
        var rooms = await _roomRepository.GetAvailableRoomsAsync(request.HotelId);

        return _mapper.Map<IEnumerable<RoomResponse>>(rooms);
    }
}