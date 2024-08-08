namespace TABP.Application.Commands.Rooms.UpdateRoom;

public class RoomUpdate
{
    public string RoomNumber { get; set; }
    public int MaxChildren { get; set; }
    public int MaxAdults { get; set; }
    public double Price { get; set; }
}