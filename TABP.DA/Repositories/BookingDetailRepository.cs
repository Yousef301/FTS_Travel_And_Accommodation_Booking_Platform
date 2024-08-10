using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.DbContexts;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class BookingDetailRepository : IBookingDetailRepository
{
    private readonly TABPDbContext _context;

    public BookingDetailRepository(TABPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BookingDetail>> GetByBookingIdAsync(Guid bookingId)
    {
        return await _context.BookingDetails
            .Where(bd => bd.BookingId == bookingId)
            .Include(bd => bd.Room)
            .ToListAsync();
    }
    
    public async Task<BookingDetail> CreateAsync(BookingDetail bookingDetail)
    {
        var createdBookingDetail = await _context.BookingDetails
            .AddAsync(bookingDetail);

        return createdBookingDetail.Entity;
    }
    

    public async Task<bool> ExistsAsync(Expression<Func<BookingDetail, bool>> predicate)
    {
        return await _context.BookingDetails.AnyAsync(predicate);
    }

    public async Task<bool> IsRoomAvailableAsync(Guid roomId, DateOnly startDate, DateOnly endDate)
    {
        return !await _context.BookingDetails
            .Where(bd => bd.RoomId == roomId)
            .Where(bd => bd.Booking.CheckInDate <= endDate && bd.Booking.CheckOutDate >= startDate)
            .AnyAsync();
    }
}