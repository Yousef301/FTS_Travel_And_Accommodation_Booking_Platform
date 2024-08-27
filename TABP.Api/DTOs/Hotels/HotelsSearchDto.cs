using TABP.Shared.Constants;

namespace TABP.Web.DTOs.Hotels;

public class HotelsSearchDto : FilterParameters
{
    public DateOnly CheckInDate { get; init; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly CheckOutDate { get; init; } = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
    public int NumberOfRooms { get; set; } = Constants.MinimumNumberOfRooms;
    public int NumberOfAdults { get; init; } = Constants.DefaultNumberOfAdults;
    public int NumberOfChildren { get; init; } = Constants.MinimumNumberOfChildren;
    public decimal MinPrice { get; init; } = Constants.MinimumPrice;
    public decimal MaxPrice { get; init; } = Constants.MaximumPrice;
    public decimal ReviewRating { get; init; } = Constants.MinimumReviewRating;
    public string? RoomType { get; init; }
    public IEnumerable<Guid> Amenities { get; init; } = new List<Guid>();
}