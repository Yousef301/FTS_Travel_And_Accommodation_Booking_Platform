using MediatR;
using TABP.Domain.Enums;
using TABP.Domain.Models;

namespace TABP.Application.Queries.Hotels.GetHotels;

public class GetHotelsQuery : IRequest<PagedList<HotelResponse>>
{
    public string? SearchString { get; init; }
    public DateOnly CheckInDate { get; init; }
    public DateOnly CheckOutDate { get; init; }
    public int NumberOfRooms { get; init; }
    public int NumberOfAdults { get; init; }
    public int NumberOfChildren { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public double MinPrice { get; init; }
    public double MaxPrice { get; init; }
    public double ReviewRating { get; init; }
    public IEnumerable<Guid>? Amenities { get; init; }
}