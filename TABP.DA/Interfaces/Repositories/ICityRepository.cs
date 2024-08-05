using System.Linq.Expressions;
using TABP.DAL.Entities;
using TABP.DAL.Models.Procedures;

namespace TABP.DAL.Interfaces.Repositories;

public interface ICityRepository
{
    Task<IEnumerable<City>> GetAsync();
    Task<IEnumerable<TrendingCities>> GetTrendingDestinations();
    Task<City?> GetByIdAsync(Guid id);
    Task<City> CreateAsync(City city);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(City city);
    Task<bool> ExistsAsync(Expression<Func<City, bool>> predicate);
}