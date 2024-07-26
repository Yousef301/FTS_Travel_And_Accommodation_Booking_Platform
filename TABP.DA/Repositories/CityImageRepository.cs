using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class CityImageRepository : ICityImageRepository
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

    public async Task<CityImage?> GetByIdAsync(Guid id)
    {
        return await _context.CityImages.FindAsync(id);
    }

    public async Task<CityImage> CreateAsync(CityImage cityImage)
    {
        var createdCityImage = await _context.CityImages
            .AddAsync(cityImage);

        return createdCityImage.Entity;
    }

    public async Task DeleteAsync(CityImage cityImage)
    {
        if (!await _context.CityImages.AnyAsync(ci => ci.Id == cityImage.Id))
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