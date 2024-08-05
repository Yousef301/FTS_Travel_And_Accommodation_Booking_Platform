using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IRoomRepository
{
    Task<IEnumerable<Room>> GetAsync();
    Task<IEnumerable<Room>> GetByHotelIdAsync(Guid hotelId);
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(Guid hotelId);
    Task<IEnumerable<Room>> GetByIdAndHotelIdAsync(Guid hotelId, IEnumerable<Guid> roomIds);
    Task<Room?> GetByIdAsync(Guid id);
    Task<Room> CreateAsync(Room room);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Room room);
    Task UpdateStatusToReservedByIdAsync(Guid id);
    Task<bool> ExistsAsync(Expression<Func<Room, bool>> predicate);
}