using System.Linq.Expressions;
using TABP.DAL.Entities;
using TABP.DAL.Models;
using TABP.Domain.Models;

namespace TABP.DAL.Interfaces.Repositories;

public interface IHotelRepository
{
    Task<PagedList<Hotel>> GetAsync(Query query, bool includeCities = false);
    Task<IEnumerable<Hotel>> GetHotelsWithDealsAsync(int count = 5);
    Task<Hotel?> GetByIdAsync(Guid id);
    Task<Hotel?> GetByIdDetailsIncludedAsync(Guid id);
    Task<double> GetHotelRateAsync(Guid id);
    Task<Hotel> CreateAsync(Hotel hotel);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Hotel hotel);
    Task UpdateRateAsync(Guid id, double rate);
    Task<bool> ExistsAsync(Expression<Func<Hotel, bool>> predicate);
}