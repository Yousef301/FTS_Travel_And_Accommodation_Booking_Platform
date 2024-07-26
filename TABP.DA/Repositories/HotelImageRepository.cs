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

    public async Task<IEnumerable<HotelImage>> GetAsync()
    {
        return await _context.HotelImages.ToListAsync();
    }

    public async Task<HotelImage?> GetByIdAsync(Guid id)
    {
        return await _context.HotelImages.FindAsync(id);
    }

    public async Task<HotelImage> CreateAsync(HotelImage hotelImage)
    {
        var createdHotelImage = await _context.HotelImages
            .AddAsync(hotelImage);

        return createdHotelImage.Entity;
    }

    public async Task DeleteAsync(HotelImage hotelImage)
    {
        if (!await _context.HotelImages.AnyAsync(hi => hi.Id == hotelImage.Id))
        {
            return;
        }

        _context.HotelImages.Remove(hotelImage);
    }

    public async Task UpdateAsync(HotelImage hotelImage)
    {
        if (!await _context.HotelImages.AnyAsync(hi => hi.Id == hotelImage.Id))
            return;

        _context.HotelImages.Update(hotelImage);
    }

    public async Task<bool> ExistsAsync(Expression<Func<HotelImage, bool>> predicate)
    {
        return await _context.HotelImages.AnyAsync(predicate);
    }
}