using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<RoomAmenity>> GetAsync()
    {
        return await _context.RoomAmenities.ToListAsync();
    }

    public async Task<RoomAmenity?> GetByIdAsync(Guid id)
    {
        return await _context.RoomAmenities.FindAsync(id);
    }

    public async Task<RoomAmenity> CreateAsync(RoomAmenity roomAmenity)
    {
        var createdRoomAmenity = await _context.RoomAmenities
            .AddAsync(roomAmenity);

        return createdRoomAmenity.Entity;
    }

    public async Task DeleteAsync(RoomAmenity roomAmenity)
    {
        if (!await _context.RoomAmenities.AnyAsync(ra => ra.Id == roomAmenity.Id))
        {
            return;
        }

        _context.RoomAmenities.Remove(roomAmenity);
    }

    public async Task UpdateAsync(RoomAmenity roomAmenity)
    {
        if (!await _context.RoomAmenities.AnyAsync(ra => ra.Id == roomAmenity.Id))
            return;

        _context.RoomAmenities.Update(roomAmenity);
    }

    public async Task<bool> ExistsAsync(Expression<Func<RoomAmenity, bool>> predicate)
    {
        return await _context.RoomAmenities.AnyAsync(predicate);
    }
}