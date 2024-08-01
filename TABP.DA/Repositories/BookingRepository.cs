using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly TABPDbContext _context;

    public BookingRepository(TABPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Booking>> GetAsync()
    {
        return await _context.Bookings.ToListAsync();
    }

    public async Task<IEnumerable<Guid>> GetRecentBookingsHotelsIdForAUser(Guid userId, int count = 5)
    {
        return await _context.Bookings
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CheckOutDate)
            .Take(count)
            .Select(b => b.HotelId)
            .ToListAsync();
    }

    public async Task<Booking?> GetByIdAsync(Guid id)
    {
        return await _context.Bookings.FindAsync(id);
    }

    public async Task<Booking> CreateAsync(Booking booking)
    {
        var createdBooking = await _context.Bookings
            .AddAsync(booking);

        return createdBooking.Entity;
    }

    public async Task DeleteAsync(Booking booking)
    {
        if (!await _context.Bookings.AnyAsync(b => b.Id == booking.Id))
        {
            return;
        }

        _context.Bookings.Remove(booking);
    }

    public async Task UpdateAsync(Booking booking)
    {
        if (!await _context.Bookings.AnyAsync(b => b.Id == booking.Id))
            return;

        _context.Bookings.Update(booking);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Booking, bool>> predicate)
    {
        return await _context.Bookings.AnyAsync(predicate);
    }
}