using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class RoomImageRepository : IImageRepository<RoomImage>
{
    private readonly TABPDbContext _context;

    public RoomImageRepository(TABPDbContext context)
    {
        _context = context;
    }

    public async Task<RoomImage?> GetByIdAsync(Expression<Func<RoomImage, bool>> predicate)
    {
        return await _context.RoomImages.FirstOrDefaultAsync(predicate);
    }

    public async Task<string?> GetImagePathAsync(Guid id, Guid roomId)
    {
        return await _context.RoomImages
            .Where(r => r.Id == id && r.RoomId == roomId)
            .Select(r => r.ImagePath)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<string>> GetImagesPathAsync(Guid id)
    {
        return await _context.RoomImages
            .Where(r => r.RoomId == id)
            .Select(r => r.ImagePath)
            .ToListAsync();
    }

    public async Task<string?> GetThumbnailPathAsync(Guid id)
    {
        return await _context.RoomImages
            .Where(r => r.RoomId == id && r.Thumbnail)
            .Select(r => r.ImagePath)
            .FirstOrDefaultAsync();
    }

    public async Task<RoomImage> CreateAsync(RoomImage roomImage)
    {
        var createdRoomImage = await _context.RoomImages
            .AddAsync(roomImage);

        return createdRoomImage.Entity;
    }

    public async Task AddRangeAsync(IEnumerable<RoomImage> roomImages)
    {
        await _context.RoomImages.AddRangeAsync(roomImages);
    }

    public async Task DeleteAsync(Guid id)
    {
        var roomImage = await _context.RoomImages.FindAsync(id);

        if (roomImage == null)
        {
            return;
        }

        _context.RoomImages.Remove(roomImage);
    }

    public async Task<bool> ExistsAsync(Expression<Func<RoomImage, bool>> predicate)
    {
        return await _context.RoomImages.AnyAsync(predicate);
    }
}