using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<BookingDetail>> GetAsync()
    {
        return await _context.BookingDetails.ToListAsync();
    }

    public async Task<IEnumerable<BookingDetail>> GetByRoomIdAsync(Guid roomId)
    {
        return await _context.BookingDetails
            .Where(bd => bd.RoomId == roomId)
            .Include(bd => bd.Booking)
            .ToListAsync();
    }

    public async Task<IEnumerable<BookingDetail>> GetByBookingIdAsync(Guid bookingId)
    {
        return await _context.BookingDetails
            .Where(bd => bd.BookingId == bookingId)
            .Include(bd => bd.Room)
            .ToListAsync();
    }

    public async Task<BookingDetail?> GetByIdAsync(Guid id)
    {
        return await _context.BookingDetails.FindAsync(id);
    }

    public async Task<BookingDetail> CreateAsync(BookingDetail bookingDetail)
    {
        var createdBookingDetail = await _context.BookingDetails
            .AddAsync(bookingDetail);

        return createdBookingDetail.Entity;
    }

    public async Task DeleteAsync(BookingDetail bookingDetail)
    {
        if (!await _context.BookingDetails.AnyAsync(bd => bd.Id == bookingDetail.Id))
        {
            return;
        }

        _context.BookingDetails.Remove(bookingDetail);
    }

    public async Task UpdateAsync(BookingDetail bookingDetail)
    {
        if (!await _context.BookingDetails.AnyAsync(bd => bd.Id == bookingDetail.Id))
            return;

        _context.BookingDetails.Update(bookingDetail);
    }

    public async Task<bool> ExistsAsync(Expression<Func<BookingDetail, bool>> predicate)
    {
        return await _context.BookingDetails.AnyAsync(predicate);
    }
}