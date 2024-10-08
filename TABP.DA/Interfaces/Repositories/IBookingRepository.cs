﻿using System.Linq.Expressions;
using TABP.DAL.Entities;
using TABP.DAL.Models;

namespace TABP.DAL.Interfaces.Repositories;

public interface IBookingRepository
{
    Task<IEnumerable<BookingDto>> GetUserBookingsAsync(Guid userId);
    Task<IEnumerable<Guid>> GetRecentlyBookedHotelsIdByUserAsync(Guid userId, int count = 5);
    Task<BookingDto?> GetDetailedByIdAsync(Guid id);
    Task<Booking?> GetByIdAsync(Guid id, bool includePayment = false);
    Task<Booking?> GetPendingBooking(Guid userId);
    Task<Booking> CreateAsync(Booking booking);
    void Delete(Booking booking);
    void Update(Booking booking);
    Task<bool> ExistsAsync(Expression<Func<Booking, bool>> predicate);
    Task<bool> IsBookingOverlapsAsync(Guid hotelId, Guid userId, DateOnly checkInDate, DateOnly checkOutDate);
}