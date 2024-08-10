using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IReviewRepository
{
    Task<IEnumerable<Review>> GetAsync();
    Task<IEnumerable<Review>> GetByHotelIdAsync(Guid hotelId);
    Task<IEnumerable<Review>> GetUserHotelsReviewsAsync(Guid hotelId, Guid userId);
    Task<int> GetHotelReviewsCount(Guid hotelId);
    Task<Review?> GetByIdAsync(Guid id);
    Task<Review> CreateAsync(Review review);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Review review);
    Task<bool> ExistsAsync(Expression<Func<Review, bool>> predicate);
}