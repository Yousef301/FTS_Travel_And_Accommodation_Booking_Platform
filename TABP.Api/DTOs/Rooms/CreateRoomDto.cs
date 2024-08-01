namespace TABP.Web.DTOs.Rooms;

public class CreateRoomDto : RoomBase
{
    public IEnumerable<Guid> AmenityIds { get; set; }
}