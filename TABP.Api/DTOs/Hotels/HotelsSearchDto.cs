using TABP.Domain.Enums;

namespace TABP.Web.DTOs.Hotels;

public class HotelsSearchDto
{
    public string? SearchString { get; set; }
    public DateOnly CheckInDate { get; init; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly CheckOutDate { get; init; } = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
    public int NumberOfRooms { get; set; } = 1;
    public int NumberOfAdults { get; init; } = 2;
    public int NumberOfChildren { get; init; } = 0;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public double MinPrice { get; init; } = 0;
    public double MaxPrice { get; init; } = 10000;
    public double ReviewRating { get; init; } = 0;
    public IEnumerable<Guid> Amenities { get; init; } = new List<Guid>();
}