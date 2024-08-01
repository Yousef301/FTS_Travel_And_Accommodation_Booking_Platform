using MediatR;

namespace TABP.Application.Queries.Rooms.GetRooms;

public class GetRoomsQuery : IRequest<IEnumerable<RoomResponse>>
{
    public Guid HotelId { get; set; }
}