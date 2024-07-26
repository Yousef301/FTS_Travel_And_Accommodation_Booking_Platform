using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IReviewRepository
{
    Task<IEnumerable<Review>> GetAsync();
    Task<Review?> GetByIdAsync(Guid id);
    Task<Review> CreateAsync(Review review);
    Task DeleteAsync(Review review);
    Task UpdateAsync(Review review);
    Task<bool> ExistsAsync(Expression<Func<Review, bool>> predicate);
}