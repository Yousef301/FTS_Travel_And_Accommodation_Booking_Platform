using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IHotelRepository
{
    Task<IEnumerable<Hotel>> GetAsync();
    Task<Hotel?> GetByIdAsync(Guid id);
    Task<Hotel> CreateAsync(Hotel hotel);
    Task DeleteAsync(Hotel hotel);
    Task UpdateAsync(Hotel hotel);
    Task<bool> ExistsAsync(Expression<Func<Hotel, bool>> predicate);
}