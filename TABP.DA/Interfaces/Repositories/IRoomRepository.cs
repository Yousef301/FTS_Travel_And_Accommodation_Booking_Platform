using System.Linq.Expressions;
using TABP.DAL.Entities;
using TABP.Domain.Models;

namespace TABP.DAL.Interfaces.Repositories;

public interface IRoomRepository
{
    Task<IEnumerable<Room>> GetAsync();
    Task<PagedList<Room>> GetByHotelIdPagedAsync(Guid hotelId, Filters<Room> filters);

    Task<IEnumerable<Room>> GetByHotelIdAsync(Guid hotelId,
        Expression<Func<Room, bool>>? predicate = null, bool includeAmenities = false);

    Task<IEnumerable<Room>> GetByIdAndHotelIdAsync(Guid hotelId, IEnumerable<Guid> roomIds);
    Task<Room?> GetByIdAsync(Guid id, Guid hotelId);
    Task<Room> CreateAsync(Room room);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Room room);
    Task UpdateStatusToReservedByIdAsync(Guid id);
    Task<bool> ExistsAsync(Expression<Func<Room, bool>> predicate);
}