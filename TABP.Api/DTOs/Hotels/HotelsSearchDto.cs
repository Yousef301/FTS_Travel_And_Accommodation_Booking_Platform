namespace TABP.Web.DTOs.Hotels;

public class HotelsSearchDto : FilterParameters
{
    public DateOnly CheckInDate { get; init; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly CheckOutDate { get; init; } = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
    public int NumberOfRooms { get; set; } = 1;
    public int NumberOfAdults { get; init; } = 2;
    public int NumberOfChildren { get; init; } = 0;
    public decimal MinPrice { get; init; } = 0;
    public decimal MaxPrice { get; init; } = 10000;
    public decimal ReviewRating { get; init; } = 0;
    public string? RoomType { get; init; }
    public IEnumerable<Guid> Amenities { get; init; } = new List<Guid>();
}