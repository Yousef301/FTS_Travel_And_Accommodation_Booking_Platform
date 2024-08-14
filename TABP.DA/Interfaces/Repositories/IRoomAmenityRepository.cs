using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IRoomAmenityRepository
{
    Task<IEnumerable<RoomAmenity>> GetRoomAmenitiesAsync(Guid roomId);
    void AddRange(IEnumerable<RoomAmenity> roomAmenities);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Expression<Func<RoomAmenity, bool>> predicate);
}