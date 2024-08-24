using System.Linq.Expressions;
using TABP.DAL.Entities;
using TABP.DAL.Models.Procedures;
using TABP.Domain.Models;

namespace TABP.DAL.Interfaces.Repositories;

public interface ICityRepository
{
    Task<PagedList<City>> GetAsync(Filters<City> filters, bool includeHotels = false);
    Task<IEnumerable<TrendingCities>> GetTrendingDestinations();
    Task<City?> GetByIdAsync(Guid id);
    Task<string?> GetThumbnailPathAsync(Guid id);
    Task<City> CreateAsync(City city);
    void Delete(City city);
    void Update(City city);
    Task<bool> ExistsAsync(Expression<Func<City, bool>> predicate);
}