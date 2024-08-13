using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.DbContexts;
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

    public async Task<IEnumerable<Review>> GetByHotelIdAsync(Guid hotelId)
    {
        return await _context.Reviews
            .AsNoTracking()
            .Where(r => r.HotelId == hotelId)
            .Include(r => r.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetUserHotelsReviewsAsync(Guid hotelId, Guid userId)
    {
        return await _context.Reviews
            .AsNoTracking()
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
        return await _context.Reviews
            .SingleOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Review> CreateAsync(Review review)
    {
        var createdReview = await _context.Reviews
            .AddAsync(review);

        return createdReview.Entity;
    }

    public void Delete(Review review)
    {
        _context.Reviews.Remove(review);
    }

    public void Update(Review review)
    {
        _context.Reviews.Update(review);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Review, bool>> predicate)
    {
        return await _context.Reviews.AnyAsync(predicate);
    }
}