using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class HotelRepository : IHotelRepository
{
    private readonly TABPDbContext _context;

    public HotelRepository(TABPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Hotel>> GetAsync()
    {
        return await _context.Hotels.ToListAsync();
    }

    public async Task<IEnumerable<Hotel>> GetIncludeCityAsync()
    {
        return await _context.Hotels
            .Include(h => h.City)
            .ToListAsync();
    }

    public async Task<Hotel?> GetByIdAsync(Guid id)
    {
        return await _context.Hotels.FindAsync(id);
    }

    public async Task<Hotel?> GetByIdDetailsIncludedAsync(Guid id)
    {
        return await _context.Hotels
            .Include(h => h.City)
            .Include(h => h.Rooms)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<Hotel> CreateAsync(Hotel hotel)
    {
        var createdHotel = await _context.Hotels
            .AddAsync(hotel);

        return createdHotel.Entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var hotel = await _context.Hotels.FindAsync(id);

        if (hotel == null)
        {
            return;
        }

        _context.Hotels.Remove(hotel);
    }

    public async Task UpdateAsync(Hotel hotel)
    {
        if (!await _context.Hotels.AnyAsync(bd => bd.Id == hotel.Id))
            return;

        _context.Hotels.Update(hotel);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Hotel, bool>> predicate)
    {
        return await _context.Hotels.AnyAsync(predicate);
    }
}