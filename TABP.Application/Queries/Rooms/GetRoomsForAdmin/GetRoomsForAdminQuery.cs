using MediatR;

namespace TABP.Application.Queries.Rooms.GetRooms;

public class GetRoomsForAdminQuery : IRequest<IEnumerable<RoomResponse>>
{
    public Guid HotelId { get; set; }
}