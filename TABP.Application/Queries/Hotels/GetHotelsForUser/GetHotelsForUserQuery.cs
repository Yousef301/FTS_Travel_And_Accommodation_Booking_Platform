using MediatR;
using TABP.Shared.Enums;
using TABP.Shared.Models;

namespace TABP.Application.Queries.Hotels.GetHotelsForUser;

public class GetHotelsForUserQuery : IRequest<PagedList<HotelUserResponse>>
{
    public string? SearchString { get; init; }
    public string? SortBy { get; init; }
    public SortOrder SortOrder { get; init; }
    public DateOnly CheckInDate { get; init; }
    public DateOnly CheckOutDate { get; init; }
    public int NumberOfRooms { get; init; }
    public int NumberOfAdults { get; init; }
    public int NumberOfChildren { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public decimal MinPrice { get; init; }
    public decimal MaxPrice { get; init; }
    public double ReviewRating { get; init; }
    public string RoomType { get; init; }
    public IEnumerable<Guid>? Amenities { get; init; }
}