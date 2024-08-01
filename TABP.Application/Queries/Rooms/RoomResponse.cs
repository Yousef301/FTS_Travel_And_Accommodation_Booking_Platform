using TABP.Application.Queries.Amenities;
using TABP.DAL.Enums;

namespace TABP.Application.Queries.Rooms;

public class RoomResponse
{
    public Guid Id { get; set; }
    public string RoomNumber { get; set; }
    public string RoomType { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public int MaxChildren { get; set; }
    public int MaxAdults { get; set; }
    public double Price { get; set; }
    public IEnumerable<AmenityResponse> Amenities { get; set; }
}