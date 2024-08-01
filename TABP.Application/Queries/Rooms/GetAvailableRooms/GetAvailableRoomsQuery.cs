using MediatR;

namespace TABP.Application.Queries.Rooms.GetAvailableRooms;

public class GetAvailableRoomsQuery : IRequest<IEnumerable<RoomResponse>>
{
    public Guid HotelId { get; set; }
}