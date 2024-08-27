using System.Linq.Expressions;
using TABP.Application.Helpers.Interfaces;
using TABP.DAL.Entities;
using TABP.Shared.Enums;

namespace TABP.Application.Helpers.Implementations;

public class HotelExpressions : IHotelExpressions
{
    public Expression<Func<Hotel, bool>> GetHotelsBasedOnCityOrHotelNameExpression(string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
        {
            return h => true;
        }

        return h => h.Name.Contains(searchString) || h.City.Name.Contains(searchString);
    }

    public Expression<Func<Hotel, bool>> GetHotelsBasedOnAdultsAndChildrenExpression(int numberOfAdults,
        int numberOfChildren)
    {
        return h => h.Rooms.Any(r => r.MaxAdults >= numberOfAdults
                                     && r.MaxChildren >= numberOfChildren);
    } // TODO: Need to add more logic

    public Expression<Func<Hotel, bool>> GetHotelsBasedOnPriceRangeExpression(decimal minPrice,
        decimal maxPrice)
    {
        return h => h.Rooms.Any(r => r.Price >= minPrice && r.Price <= maxPrice);
    }

    public Expression<Func<Hotel, bool>> GetHotelsBasedOnNumberOfAvailableRoomsAndDatesExpression(
        int numberOfRooms,
        DateOnly checkInDate,
        DateOnly checkOutDate)
    {
        return h => h.Rooms.Count(r => !r.BookingDetails.Any(bd =>
            bd.Booking.CheckInDate < checkOutDate &&
            bd.Booking.CheckOutDate > checkInDate)) >= numberOfRooms;
    }

    public Expression<Func<Hotel, bool>> GetHotelsBasedOnReviewRatingExpression(double reviewRating)
    {
        if (reviewRating == 0)
        {
            return h => true;
        }

        return h => h.Rating >= reviewRating;
    }

    public Expression<Func<Hotel, bool>> GetHotelsBasedOnAmenitiesExpression(IEnumerable<Guid> amenities)
    {
        var amenitiesList = amenities.ToList();

        if (!amenitiesList.Any())
        {
            return h => true;
        }

        return h => h.Rooms.Any(r =>
            r.RoomAmenities.Any(ra => amenitiesList.Contains(ra.AmenityId)));
    }

    public Expression<Func<Hotel, bool>> GetHotelsBasedOnRoomTypeExpression(string roomType)
    {
        if (Enum.TryParse(roomType, true, out RoomType result))
        {
            return h => h.Rooms.Any(r => r.RoomType == result);
        }

        return h => true;
    }

    public Expression<Func<Hotel, object>> GetSortExpression(string? sortBy)
    {
        return sortBy?.ToLower() switch
        {
            "price" => h => h.Rooms.Min(r => r.Price),
            "rating" => h => h.Rating,
            _ => h => h.Name
        };
    }
}