using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class HotelImageRepository : IImageRepository<HotelImage>
{
    private readonly TABPDbContext _context;

    public HotelImageRepository(TABPDbContext context)
    {
        _context = context;
    }


    public async Task<HotelImage?> GetByIdAsync(Guid id)
    {
        return await _context.HotelImages.FindAsync(id);
    }

    public async Task<string?> GetImagePathAsync(Guid id, Guid hotelId)
    {
        return await _context.HotelImages
            .Where(h => h.Id == id && h.HotelId == hotelId)
            .Select(h => h.ImagePath)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<string>> GetImagesPathAsync(Guid id)
    {
        return await _context.HotelImages
            .Where(h => h.HotelId == id)
            .Select(h => h.ImagePath)
            .ToListAsync();
    }

    public async Task<string?> GetThumbnailPathAsync(Guid id)
    {
        return await _context.HotelImages
            .Where(h => h.HotelId == id && h.Thumbnail)
            .Select(h => h.ImagePath)
            .FirstOrDefaultAsync();
    }

    public async Task<HotelImage> CreateAsync(HotelImage hotelImage)
    {
        var createdHotelImage = await _context.HotelImages
            .AddAsync(hotelImage);

        return createdHotelImage.Entity;
    }

    public async Task AddRangeAsync(IEnumerable<HotelImage> hotelImages)
    {
        await _context.HotelImages.AddRangeAsync(hotelImages);
    }

    public async Task DeleteAsync(Guid id)
    {
        var hotelImage = await _context.HotelImages.FindAsync(id);

        if (hotelImage == null)
        {
            return;
        }

        _context.HotelImages.Remove(hotelImage);
    }

    public async Task<bool> ExistsAsync(Expression<Func<HotelImage, bool>> predicate)
    {
        return await _context.HotelImages.AnyAsync(predicate);
    }
}