using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.Application.Helpers.Interfaces;

public interface IHotelExpressions
{
    public Expression<Func<Hotel, bool>> GetHotelsBasedOnCityOrHotelNameExpression(string searchString);

    public Expression<Func<Hotel, bool>> GetHotelsBasedOnAdultsAndChildrenExpression(int numberOfAdults,
        int numberOfChildren);

    public Expression<Func<Hotel, bool>> GetHotelsBasedOnPriceRangeExpression(decimal minPrice,
        decimal maxPrice);

    public Expression<Func<Hotel, bool>> GetHotelsBasedOnNumberOfAvailableRoomsAndDatesExpression(int numberOfRooms,
        DateOnly checkInDate, DateOnly checkOutDate);

    public Expression<Func<Hotel, bool>> GetHotelsBasedOnReviewRatingExpression(double reviewRating);
    public Expression<Func<Hotel, bool>> GetHotelsBasedOnAmenitiesExpression(IEnumerable<Guid> amenities);
    public Expression<Func<Hotel, bool>> GetHotelsBasedOnRoomTypeExpression(string roomType);
    public Expression<Func<Hotel, object>> GetSortExpression(string? sortBy);
}