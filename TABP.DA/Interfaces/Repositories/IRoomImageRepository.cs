using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IRoomImageRepository
{
    Task<IEnumerable<RoomImage>> GetAsync();
    Task<RoomImage?> GetByIdAsync(Guid id);
    Task<RoomImage> CreateAsync(RoomImage roomImage);
    Task DeleteAsync(RoomImage roomImage);
    Task UpdateAsync(RoomImage roomImage);
    Task<bool> ExistsAsync(Expression<Func<RoomImage, bool>> predicate);
}