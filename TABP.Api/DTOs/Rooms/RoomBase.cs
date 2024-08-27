namespace TABP.Web.DTOs.Rooms;

public class RoomBase
{
    public string RoomNumber { get; set; }
    public int MaxChildren { get; set; }
    public int MaxAdults { get; set; }
    public decimal Price { get; set; }
}