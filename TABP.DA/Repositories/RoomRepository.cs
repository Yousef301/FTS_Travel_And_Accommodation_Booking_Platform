using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Enums;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly TABPDbContext _context;

    public RoomRepository(TABPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Room>> GetAsync()
    {
        return await _context.Rooms.ToListAsync();
    }

    public async Task<IEnumerable<Room>> GetByHotelAsync(Guid id)
    {
        return await _context.Rooms
            .Where(r => r.HotelId == id)
            .Include(r => r.RoomAmenities)
            .ThenInclude(ra => ra.Amenity)
            .ToListAsync();
    }

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(Guid id)
    {
        return await _context.Rooms
            .Where(r => r.HotelId == id && r.Status == RoomStatus.Available)
            .ToListAsync();
    }

    public async Task<Room?> GetByIdAsync(Guid id)
    {
        return await _context.Rooms.FindAsync(id);
    }

    public async Task<Room> CreateAsync(Room room)
    {
        var createdRoom = await _context.Rooms
            .AddAsync(room);

        return createdRoom.Entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
        {
            return;
        }

        _context.Rooms.Remove(room);
    }

    public async Task UpdateAsync(Room room)
    {
        if (!await _context.Rooms.AnyAsync(r => r.Id == room.Id))
            return;

        _context.Rooms.Update(room);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Room, bool>> predicate)
    {
        return await _context.Rooms.AnyAsync(predicate);
    }
}