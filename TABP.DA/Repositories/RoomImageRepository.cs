using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class RoomImageRepository : IRoomImageRepository
{
    private readonly TABPDbContext _context;

    public RoomImageRepository(TABPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoomImage>> GetAsync()
    {
        return await _context.RoomImages.ToListAsync();
    }

    public async Task<RoomImage?> GetByIdAsync(Guid id)
    {
        return await _context.RoomImages.FindAsync(id);
    }

    public async Task<RoomImage> CreateAsync(RoomImage roomImage)
    {
        var createdRoomImages = await _context.RoomImages
            .AddAsync(roomImage);

        return createdRoomImages.Entity;
    }

    public async Task DeleteAsync(RoomImage roomImage)
    {
        if (!await _context.RoomImages.AnyAsync(ri => ri.Id == roomImage.Id))
        {
            return;
        }

        _context.RoomImages.Remove(roomImage);
    }

    public async Task UpdateAsync(RoomImage roomImage)
    {
        if (!await _context.RoomImages.AnyAsync(ri => ri.Id == roomImage.Id))
            return;

        _context.RoomImages.Update(roomImage);
    }

    public async Task<bool> ExistsAsync(Expression<Func<RoomImage, bool>> predicate)
    {
        return await _context.RoomImages.AnyAsync(predicate);
    }
}