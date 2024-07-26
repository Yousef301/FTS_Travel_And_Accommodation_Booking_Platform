using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IRoomAmenityRepository
{
    Task<IEnumerable<RoomAmenity>> GetAsync();
    Task<RoomAmenity?> GetByIdAsync(Guid id);
    Task<RoomAmenity> CreateAsync(RoomAmenity roomAmenity);
    Task DeleteAsync(RoomAmenity roomAmenity);
    Task UpdateAsync(RoomAmenity roomAmenity);
    Task<bool> ExistsAsync(Expression<Func<RoomAmenity, bool>> predicate);
}