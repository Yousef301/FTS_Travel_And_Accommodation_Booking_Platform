using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IBookingDetailRepository
{
    Task<IEnumerable<BookingDetail>> GetByBookingIdAsync(Guid bookingId, bool includeRoom = false);
    Task<BookingDetail> CreateAsync(BookingDetail bookingDetail);
    Task AddRangeAsync(List<BookingDetail> bookingsDetails);
    Task<bool> ExistsAsync(Expression<Func<BookingDetail, bool>> predicate);
    Task<bool> IsRoomAvailableAsync(Guid roomId, DateOnly startDate, DateOnly endDate);
}