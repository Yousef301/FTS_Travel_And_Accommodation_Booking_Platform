using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IAmenityRepository
{
    Task<IEnumerable<Amenity>> GetAsync();
    Task<Amenity?> GetByIdAsync(Guid id);
    Task<Amenity> CreateAsync(Amenity amenity);
    void Delete(Amenity amenity);
    void Update(Amenity amenity);
    Task<bool> ExistsAsync(Expression<Func<Amenity, bool>> predicate);
}