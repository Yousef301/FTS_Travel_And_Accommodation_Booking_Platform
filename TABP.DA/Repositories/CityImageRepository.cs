using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.DbContexts;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class CityImageRepository : IImageRepository<CityImage>
{
    private readonly TABPDbContext _context;

    public CityImageRepository(TABPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CityImage>> GetAsync()
    {
        return await _context.CityImages.ToListAsync();
    }

    public async Task<CityImage?> GetByIdAsync(Expression<Func<CityImage, bool>> predicate)
    {
        return await _context.CityImages.FirstOrDefaultAsync(predicate);
    }

    public async Task<string?> GetImagePathAsync(Guid imageId, Guid cityId)
    {
        return await _context.CityImages
            .Where(ci => ci.Id == imageId && ci.CityId == cityId)
            .Select(ci => ci.ImagePath)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<string>> GetImagesPathAsync(Guid id)
    {
        return await _context.CityImages
            .Where(ci => ci.CityId == id)
            .Select(ci => ci.ImagePath)
            .ToListAsync();
    }

    public async Task<string?> GetThumbnailPathAsync(Guid cityId)
    {
        return await _context.CityImages
            .Where(h => h.CityId == cityId && h.Thumbnail)
            .Select(h => h.ImagePath)
            .FirstOrDefaultAsync();
    }

    public async Task<CityImage> CreateAsync(CityImage cityImage)
    {
        var createdCityImage = await _context.CityImages
            .AddAsync(cityImage);

        return createdCityImage.Entity;
    }

    public async Task AddRangeAsync(IEnumerable<CityImage> cityImages)
    {
        await _context.CityImages.AddRangeAsync(cityImages);
    }

    public async Task DeleteAsync(Guid id)
    {
        var cityImage = await _context.CityImages.FindAsync(id);

        if (cityImage == null)
        {
            return;
        }

        _context.CityImages.Remove(cityImage);
    }

    public async Task UpdateAsync(CityImage cityImage)
    {
        if (!await _context.CityImages.AnyAsync(ci => ci.Id == cityImage.Id))
            return;

        _context.CityImages.Update(cityImage);
    }

    public async Task<bool> ExistsAsync(Expression<Func<CityImage, bool>> predicate)
    {
        return await _context.CityImages.AnyAsync(predicate);
    }
}