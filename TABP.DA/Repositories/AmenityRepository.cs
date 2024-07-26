using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class AmenityRepository : IAmenityRepository
{
    private readonly TABPDbContext _context;

    public AmenityRepository(TABPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Amenity>> GetAsync()
    {
        return await _context.Amenities.ToListAsync();
    }

    public async Task<Amenity?> GetByIdAsync(Guid id)
    {
        return await _context.Amenities.FindAsync(id);
    }

    public async Task<Amenity> CreateAsync(Amenity amenity)
    {
        var createdAmenity = await _context.Amenities
            .AddAsync(amenity);

        return createdAmenity.Entity;
    }

    public async Task DeleteAsync(Amenity amenity)
    {
        if (!await _context.Amenities.AnyAsync(a => a.Id == amenity.Id))
        {
            return;
        }

        _context.Amenities.Remove(amenity);
    }

    public async Task UpdateAsync(Amenity amenity)
    {
        if (!await _context.Amenities.AnyAsync(a => a.Id == amenity.Id))
            return;
        
        _context.Amenities.Update(amenity);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Amenity, bool>> predicate)
    {
        return await _context.Amenities.AnyAsync(predicate);
    }
}