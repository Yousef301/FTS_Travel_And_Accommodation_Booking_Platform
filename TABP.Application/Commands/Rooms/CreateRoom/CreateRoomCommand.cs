using MediatR;
using TABP.Application.Queries.Rooms;

namespace TABP.Application.Commands.Rooms.CreateRoom;

public class CreateRoomCommand : IRequest<RoomResponse>
{
    public Guid HotelId { get; set; }
    public string RoomNumber { get; set; }
    public string RoomType { get; set; }
    public string Description { get; set; }
    public int MaxChildren { get; set; }
    public int MaxAdults { get; set; }
    public double Price { get; set; }
    public IEnumerable<Guid> AmenityIds { get; set; }
}