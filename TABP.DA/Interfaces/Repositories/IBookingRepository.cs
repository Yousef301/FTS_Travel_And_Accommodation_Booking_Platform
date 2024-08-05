using System.Linq.Expressions;
using TABP.DAL.Entities;
using TABP.DAL.Models;

namespace TABP.DAL.Interfaces.Repositories;

public interface IBookingRepository
{
    Task<IEnumerable<Booking>> GetAsync();
    Task<IEnumerable<BookingDto>> GetUserBookingsAsync(Guid userId);
    Task<IEnumerable<Guid>> GetHotelsIdsForAUserBookingsAsync(Guid userId, int count = 5);
    Task<BookingDto?> GetDetailedByIdAsync(Guid id);
    Task<Booking?> GetByIdAsync(Guid id);
    Task<Booking?> GetPendingBooking(Guid userId);
    Task<Booking> CreateAsync(Booking booking);
    Task DeleteAsync(Booking booking);
    Task UpdateAsync(Booking booking);
    Task<bool> ExistsAsync(Expression<Func<Booking, bool>> predicate);
    Task<bool> ExistsAsync(Guid hotelId, Guid userId);
    Task<bool> IsBookingOverlappingAsync(Guid hotelId, Guid userId, DateOnly checkInDate, DateOnly checkOutDate);
}