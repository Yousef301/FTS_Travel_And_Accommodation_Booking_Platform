namespace TABP.Application.Commands.Rooms.UpdateRoom;

public class RoomUpdate
{
    public string RoomNumber { get; set; }
    public string Status { get; set; }
    public string RoomType { get; set; }
    public string Description { get; set; }
    public int MaxChildren { get; set; }
    public int MaxAdults { get; set; }
    public double Price { get; set; }
}