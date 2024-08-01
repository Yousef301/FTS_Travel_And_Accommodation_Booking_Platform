﻿using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IBookingRepository
{
    Task<IEnumerable<Booking>> GetAsync();
    Task<IEnumerable<Guid>> GetRecentBookingsHotelsIdForAUser(Guid userId, int count = 5);
    Task<Booking?> GetByIdAsync(Guid id);
    Task<Booking> CreateAsync(Booking booking);
    Task DeleteAsync(Booking booking);
    Task UpdateAsync(Booking booking);
    Task<bool> ExistsAsync(Expression<Func<Booking, bool>> predicate);
}