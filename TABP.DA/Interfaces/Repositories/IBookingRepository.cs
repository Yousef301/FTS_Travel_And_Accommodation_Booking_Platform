using System.Linq.Expressions;
using TABP.DAL.Entities;
using TABP.DAL.Models;

namespace TABP.DAL.Interfaces.Repositories;

public interface IBookingRepository
{
    Task<IEnumerable<Booking>> GetAsync();
    Task<IEnumerable<BookingDto>> GetUserBookingsAsync(Guid userId);
    Task<IEnumerable<Guid>> GetRecentlyBookedHotelsIdByUserAsync(Guid userId, int count = 5);
    Task<BookingDto?> GetDetailedByIdAsync(Guid id, Guid userId);
    Task<Booking?> GetByIdAsync(Guid id, Guid userId);
    Task<Booking?> GetPendingBooking(Guid userId);
    Task<Booking> CreateAsync(Booking booking);
    Task DeleteAsync(Booking booking);
    Task UpdateAsync(Booking booking);
    Task<bool> ExistsAsync(Expression<Func<Booking, bool>> predicate);
    Task<bool> ExistsAsync(Guid hotelId, Guid userId);
    Task<bool> IsBookingOverlapsAsync(Guid hotelId, Guid userId, DateOnly checkInDate, DateOnly checkOutDate);
}