using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IRoomRepository
{
    Task<IEnumerable<Room>> GetAsync();
    Task<Room?> GetByIdAsync(Guid id);
    Task<Room> CreateAsync(Room room);
    Task DeleteAsync(Room room);
    Task UpdateAsync(Room room);
    Task<bool> ExistsAsync(Expression<Func<Room, bool>> predicate);
}