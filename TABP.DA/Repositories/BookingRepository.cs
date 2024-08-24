using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.DbContexts;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.DAL.Models;
using TABP.Domain.Enums;

namespace TABP.DAL.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly TABPDbContext _context;

    public BookingRepository(TABPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BookingDto>> GetUserBookingsAsync(Guid userId)
    {
        return await _context.Bookings
            .AsNoTracking()
            .Where(b => b.UserId == userId)
            .Select(b => new BookingDto
            {
                Id = b.Id,
                HotelName = b.Hotel.Name,
                BookingDate = b.BookingDate,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                TotalPrice = b.TotalPrice,
                BookingStatus = b.BookingStatus,
                PaymentStatus = b.PaymentStatus,
                PaymentMethod = b.PaymentMethod
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<Guid>> GetRecentlyBookedHotelsIdByUserAsync(
        Guid userId,
        int count = 5)
    {
        return await _context.Bookings
            .AsNoTracking()
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CheckOutDate)
            .Select(b => b.HotelId)
            .Distinct()
            .Take(count)
            .ToListAsync();
    }

    public async Task<BookingDto?> GetDetailedByIdAsync(Guid id)
    {
        return await _context.Bookings
            .AsNoTracking()
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
                TotalPrice = b.TotalPrice,
                UserId = b.UserId
            })
            .SingleOrDefaultAsync();
    }

    public async Task<Booking?> GetByIdAsync(Guid id, bool includePayment = false)
    {
        return includePayment
            ? await _context.Bookings
                .Include(b => b.Payment)
                .SingleOrDefaultAsync(b => b.Id == id)
            : await _context.Bookings
                .SingleOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Booking?> GetPendingBooking(Guid userId)
    {
        return await _context.Bookings
            .SingleOrDefaultAsync(b => b.UserId == userId && b.BookingStatus == BookingStatus.Pending);
    }

    public async Task<Booking> CreateAsync(Booking booking)
    {
        var createdBooking = await _context.Bookings
            .AddAsync(booking);

        return createdBooking.Entity;
    }

    public void Delete(Booking booking)
    {
        _context.Bookings.Remove(booking);
    }

    public void Update(Booking booking)
    {
        _context.Bookings.Update(booking);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Booking, bool>> predicate)
    {
        return await _context.Bookings.AnyAsync(predicate);
    }

    public async Task<bool> IsBookingOverlapsAsync(Guid hotelId, Guid userId, DateOnly checkInDate,
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