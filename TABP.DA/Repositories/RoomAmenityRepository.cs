using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.DbContexts;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class RoomAmenityRepository : IRoomAmenityRepository
{
    private readonly TABPDbContext _context;

    public RoomAmenityRepository(TABPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoomAmenity>> GetRoomAmenitiesAsync(Guid roomId)
    {
        return await _context.RoomAmenities
            .AsNoTracking()
            .Where(ra => ra.RoomId == roomId)
            .Include(ra => ra.Amenity)
            .ToListAsync();
    }

    public async Task AddRangeAsync(IEnumerable<RoomAmenity> roomAmenities)
    {
        await _context.RoomAmenities.AddRangeAsync(roomAmenities);
    }

    public async Task DeleteAsync(Guid id)
    {
        var roomAmenity = await _context.RoomAmenities.FindAsync(id);

        if (roomAmenity == null)
        {
            return;
        }

        _context.RoomAmenities.Remove(roomAmenity);
    }

    public async Task<bool> ExistsAsync(Expression<Func<RoomAmenity, bool>> predicate)
    {
        return await _context.RoomAmenities.AnyAsync(predicate);
    }
}