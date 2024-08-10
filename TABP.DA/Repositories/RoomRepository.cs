using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Enums;
using TABP.Domain.Models;

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

    public async Task<PagedList<Room>> GetByHotelIdPagedAsync(Guid hotelId, Filters<Room> filters)
    {
        var roomsQuery = _context.Rooms
            .Where(r => r.HotelId == hotelId)
            .AsQueryable();

        roomsQuery = roomsQuery.Where(filters.FilterExpression!);

        roomsQuery = filters.SortOrder == SortOrder.DESC
            ? roomsQuery.OrderByDescending(filters.SortExpression!)
            : roomsQuery.OrderBy(filters.SortExpression!);

        var rooms = await PagedList<Room>.CreateAsync(
            roomsQuery,
            filters.Page,
            filters.PageSize);

        return rooms;
    }

    public async Task<IEnumerable<Room>> GetByHotelIdAsync(Guid hotelId,
        Expression<Func<Room, bool>>? predicate = null, bool includeAmenities = false)
    {
        var query = _context.Rooms
            .Where(r => r.HotelId == hotelId);

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (includeAmenities)
        {
            query = query.Include(r => r.RoomAmenities)
                .ThenInclude(ra => ra.Amenity);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Room>> GetByIdAndHotelIdAsync(Guid hotelId, IEnumerable<Guid> roomIds)
    {
        var roomsWithOffers = await _context.Rooms
            .Where(r => r.HotelId == hotelId && roomIds.Contains(r.Id))
            .Select(r => new
            {
                Room = r,
                SpecialOffer = _context.SpecialOffers
                    .SingleOrDefault(so => so.RoomId == r.Id && so.IsActive)
            })
            .ToListAsync();

        foreach (var item in roomsWithOffers)
        {
            item.Room.SpecialOffers = item.SpecialOffer != null
                ? new List<SpecialOffer> { item.SpecialOffer }
                : new List<SpecialOffer>();
        }

        return roomsWithOffers.Select(x => x.Room);
    }

    public async Task<Room?> GetByIdAsync(Guid id, Guid hotelId)
    {
        return await _context.Rooms
            .SingleOrDefaultAsync(r => r.Id == id && r.HotelId == hotelId);
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

    public async Task UpdateStatusToReservedByIdAsync(Guid id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
            return;

        room.Status = RoomStatus.Reserved;

        _context.Rooms.Update(room);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Room, bool>> predicate)
    {
        return await _context.Rooms.AnyAsync(predicate);
    }
}