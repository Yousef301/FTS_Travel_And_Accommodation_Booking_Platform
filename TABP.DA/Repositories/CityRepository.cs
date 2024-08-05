using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.DAL.Models.Procedures;

namespace TABP.DAL.Repositories;

public class CityRepository : ICityRepository
{
    private readonly TABPDbContext _context;

    public CityRepository(TABPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<City>> GetAsync()
    {
        return await _context.Cities.ToListAsync();
    }

    public async Task<IEnumerable<TrendingCities>> GetTrendingDestinations()
    {
        return await _context.GetTrendingCitiesAsync();
    }

    public async Task<City?> GetByIdAsync(Guid id)
    {
        return await _context.Cities.FindAsync(id);
    }

    public async Task<City> CreateAsync(City city)
    {
        var createdCity = await _context.Cities
            .AddAsync(city);

        return createdCity.Entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var city = await _context.Cities.FindAsync(id);

        if (city == null)
        {
            return;
        }

        _context.Cities.Remove(city);
    }

    public async Task UpdateAsync(City city)
    {
        if (!await _context.Cities.AnyAsync(c => c.Id == city.Id))
            return;

        _context.Cities.Update(city);
    }

    public async Task<bool> ExistsAsync(Expression<Func<City, bool>> predicate)
    {
        return await _context.Cities.AnyAsync(predicate);
    }
}