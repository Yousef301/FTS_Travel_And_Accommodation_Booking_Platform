using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Enums;
using TABP.DAL.Interfaces.Repositories;
using TABP.DAL.Models;

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

    public async Task<IEnumerable<BookingDto>> GetUserBookingsAsync(Guid userId)
    {
        return await _context.Bookings
            .Where(b => b.UserId == userId)
            .Select(b => new BookingDto
            {
                Id = b.Id,
                HotelName = b.Hotel.Name,
                BookingDate = b.BookingDate,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                TotalPrice = b.TotalPrice
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<Guid>> GetHotelsIdsForAUserBookingsAsync(Guid userId, int count = 5)
    {
        return await _context.Bookings
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CheckOutDate)
            .Take(count)
            .Select(b => b.HotelId)
            .ToListAsync();
    }

    public async Task<BookingDto?> GetDetailedByIdAsync(Guid id)
    {
        return await _context.Bookings
            .Where(b => b.Id == id)
            .Select(b => new BookingDto
            {
                Id = b.Id,
                HotelName = b.Hotel.Name,
                BookingDate = b.BookingDate,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                BookingStatus = b.BookingStatus,
                PaymentMethod = b.PaymentMethod,
                PaymentStatus = b.PaymentStatus,
                TotalPrice = b.TotalPrice
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Booking?> GetByIdAsync(Guid id)
    {
        return await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Booking?> GetPendingBooking(Guid userId)
    {
        return await _context.Bookings
            .FirstOrDefaultAsync(b => b.UserId == userId && b.BookingStatus == BookingStatus.Pending);
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

    public async Task<bool> ExistsAsync(Guid hotelId, Guid userId)
    {
        return await _context.Bookings.AnyAsync(b => b.HotelId == hotelId && b.UserId == userId);
    }

    public async Task<bool> IsBookingOverlappingAsync(Guid hotelId, Guid userId, DateOnly checkInDate,
        DateOnly checkOutDate)
    {
        return await _context.Bookings
            .AnyAsync(b => b.HotelId == hotelId &&
                           b.UserId == userId &&
                           b.BookingStatus == BookingStatus.Confirmed &&
                           b.CheckInDate < checkOutDate &&
                           b.CheckOutDate > checkInDate);
    }
}