using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class HotelImageRepository : IHotelImageRepository
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

    public async Task<string?> GetImagePathAsync(Guid id)
    {
        return await _context.HotelImages
            .Where(h => h.Id == id)
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
        var hotelName = await _context.Hotels
            .Where(h => h.Id == id)
            .Select(h => h.Name)
            .FirstOrDefaultAsync();

        if (hotelName == null)
            return null;

        var hotelImages = await _context.HotelImages
            .Where(h => h.HotelId == id)
            .ToListAsync();

        return hotelImages
            .Where(h => h.ImagePath.Contains($"{hotelName}_thumbnail.", StringComparison.OrdinalIgnoreCase))
            .Select(h => h.ImagePath)
            .FirstOrDefault();
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
}