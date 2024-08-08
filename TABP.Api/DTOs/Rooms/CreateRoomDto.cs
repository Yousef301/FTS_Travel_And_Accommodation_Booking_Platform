namespace TABP.Web.DTOs.Rooms;

public class CreateRoomDto : RoomBase
{
    public string RoomType { get; set; }
    public string Description { get; set; }
}