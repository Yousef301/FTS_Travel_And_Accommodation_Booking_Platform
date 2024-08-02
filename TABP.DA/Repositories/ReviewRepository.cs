using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly TABPDbContext _context;

    public ReviewRepository(TABPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Review>> GetAsync()
    {
        return await _context.Reviews.ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetByHotelIdAsync(Guid hotelId)
    {
        return await _context.Reviews
            .Where(r => r.HotelId == hotelId)
            .Include(r => r.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetHotelReviewsForUserAsync(Guid hotelId, Guid userId)
    {
        return await _context.Reviews
            .Where(r => r.HotelId == hotelId && r.UserId == userId)
            .Include(r => r.User)
            .ToListAsync();
    }

    public async Task<int> GetHotelReviewsCount(Guid hotelId)
    {
        return await _context.Reviews
            .CountAsync(r => r.HotelId == hotelId);
    }

    public async Task<Review?> GetByIdAsync(Guid id)
    {
        return await _context.Reviews.FindAsync(id);
    }

    public async Task<Review> CreateAsync(Review review)
    {
        var createdReview = await _context.Reviews
            .AddAsync(review);

        return createdReview.Entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var review = await _context.Reviews.FindAsync(id);

        if (review == null)
        {
            return;
        }

        _context.Reviews.Remove(review);
    }

    public async Task UpdateAsync(Review review)
    {
        if (!await _context.Reviews.AnyAsync(r => r.Id == review.Id))
            return;

        _context.Reviews.Update(review);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Review, bool>> predicate)
    {
        return await _context.Reviews.AnyAsync(predicate);
    }

    public async Task<bool> ExistsAsync(Guid hotelId, Guid userId)
    {
        return await _context.Reviews
            .AnyAsync(r => r.HotelId == hotelId && r.UserId == userId);
    }
}