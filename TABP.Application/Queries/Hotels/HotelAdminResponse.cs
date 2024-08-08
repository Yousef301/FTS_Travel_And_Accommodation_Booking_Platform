namespace TABP.Application.Queries.Hotels;

public class HotelAdminResponse : HotelResponseBase
{
    public string City { get; set; }
    public string Owner { get; set; }
    public int NumberOfRooms { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}