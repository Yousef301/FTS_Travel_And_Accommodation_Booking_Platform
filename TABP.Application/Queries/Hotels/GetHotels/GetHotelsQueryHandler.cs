using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using TABP.Application.Extensions;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Enums;
using TABP.Domain.Models;

namespace TABP.Application.Queries.Hotels.GetHotels;

public class GetHotelsQueryHandler : IRequestHandler<GetHotelsQuery, PagedList<HotelResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;


    public GetHotelsQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<HotelResponse>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
    {
        var hotels = await _hotelRepository.GetAsync(new Filters<Hotel>
        {
            Page = request.Page,
            PageSize = request.PageSize,
            SearchString = request.SearchString,
            FilterExpression = GetSearchExpression(request),
            SortExpression = GetSortExpression(request)
        }, true, true);

        return _mapper.Map<PagedList<HotelResponse>>(hotels);
    }

    private Expression<Func<Hotel, bool>> GetSearchExpression(GetHotelsQuery request)
    {
        return GetHotelsBasedOnCityOrHotelNameExpression(request.SearchString)
            .And(GetHotelsBasedOnAdultsAndChildrenExpression(request.NumberOfAdults, request.NumberOfChildren))
            .And(GetHotelsBasedOnPriceRangeExpression(request.MinPrice, request.MaxPrice))
            .And(GetHotelsBasedOnNumberOfAvailableRoomsAndDatesExpression(request.NumberOfRooms, request.CheckInDate,
                request.CheckOutDate))
            .And(GetHotelsBasedOnReviewRatingExpression(request.ReviewRating))
            .And(GetHotelsBasedOnAmenitiesExpression(request.Amenities));
    }

    private Expression<Func<Hotel, bool>> GetHotelsBasedOnCityOrHotelNameExpression(string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
        {
            return h => true;
        }

        return h => h.Name.Contains(searchString) || h.City.Name.Contains(searchString);
    }

    private Expression<Func<Hotel, bool>> GetHotelsBasedOnAdultsAndChildrenExpression(int numberOfAdults,
        int numberOfChildren)
    {
        return h => h.Rooms.Any(r => r.MaxAdults >= numberOfAdults
                                     && r.MaxChildren >= numberOfChildren);
    } // TODO: Need to add more logic

    private Expression<Func<Hotel, bool>> GetHotelsBasedOnPriceRangeExpression(double minPrice, double maxPrice)
    {
        return h => h.Rooms.Any(r => r.Price >= minPrice && r.Price <= maxPrice);
    }

    private Expression<Func<Hotel, bool>> GetHotelsBasedOnNumberOfAvailableRoomsAndDatesExpression(int numberOfRooms,
        DateOnly checkInDate, DateOnly checkOutDate)
    {
        return h => h.Rooms.Count(r => !r.BookingDetails.Any(bd =>
            bd.Booking.CheckInDate < checkOutDate &&
            bd.Booking.CheckOutDate > checkInDate)) >= numberOfRooms;
    }

    private Expression<Func<Hotel, bool>> GetHotelsBasedOnReviewRatingExpression(double reviewRating)
    {
        if (reviewRating == 0)
        {
            return h => true;
        }

        return h => h.Rating >= reviewRating;
    }

    private Expression<Func<Hotel, bool>> GetHotelsBasedOnAmenitiesExpression(IEnumerable<Guid> amenities)
    {
        if (!amenities.Any())
        {
            return h => true;
        }

        return h => h.Rooms.Any(r =>
            r.RoomAmenities.Any(ra => amenities.Contains(ra.AmenityId)));
    }

    private Expression<Func<Hotel, object>> GetSortExpression(GetHotelsQuery request)
    {
        return request.SortBy?.ToLower() switch
        {
            "price" => h => h.Rooms.Min(r => r.Price),
            "rating" => h => h.Rating,
            _ => h => h.Name
        };
    }
}