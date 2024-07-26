using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface ICityRepository
{
    Task<IEnumerable<City>> GetAsync();
    Task<City?> GetByIdAsync(Guid id);
    Task<City> CreateAsync(City city);
    Task DeleteAsync(City city);
    Task UpdateAsync(City city);
    Task<bool> ExistsAsync(Expression<Func<City, bool>> predicate);
}