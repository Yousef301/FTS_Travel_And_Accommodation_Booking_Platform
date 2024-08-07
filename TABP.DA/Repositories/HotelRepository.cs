using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.DAL.Models;
using TABP.Domain.Enums;
using TABP.Domain.Models;

namespace TABP.DAL.Repositories;

public class HotelRepository : IHotelRepository
{
    private readonly TABPDbContext _context;

    public HotelRepository(TABPDbContext context)
    {
        _context = context;
    }

    public async Task<PagedList<Hotel>> GetAsync(Filters<Hotel> filters, bool includeCity = false,
        bool includeRooms = false)
    {
        var hotelsQuery = _context.Hotels.AsQueryable();

        hotelsQuery = hotelsQuery.Where(filters.FilterExpression!);

        if (includeCity)
        {
            hotelsQuery = hotelsQuery.Include(h => h.City);
        }

        if (includeRooms)
        {
            hotelsQuery = hotelsQuery.Include(h => h.Rooms);
        }

        hotelsQuery = filters.SortOrder == SortOrder.DESC
            ? hotelsQuery.OrderByDescending(filters.SortExpression!)
            : hotelsQuery.OrderBy(filters.SortExpression!);

        var hotels = await PagedList<Hotel>.CreateAsync(
            hotelsQuery,
            filters.Page,
            filters.PageSize
        );

        return hotels;
    }

    public async Task<IEnumerable<Hotel>> GetHotelsWithDealsAsync(int count = 5) // TODO: MAKE IT FASTER IF POSSIBLE
    {
        var hotels = await _context.Hotels
            .Include(h => h.Rooms)
            .ThenInclude(r => r.SpecialOffers)
            .Where(h => h.Rooms.Any(r => r.SpecialOffers.Any(so => so.IsActive)))
            .Select(h => new Hotel
            {
                Id = h.Id,
                CityId = h.CityId,
                Name = h.Name,
                Owner = h.Owner,
                Address = h.Address,
                Description = h.Description,
                PhoneNumber = h.PhoneNumber,
                Email = h.Email,
                Rating = h.Rating,
                Rooms = h.Rooms
                    .Where(r => r.SpecialOffers.Any(so => so.IsActive))
                    .OrderByDescending(r => r.SpecialOffers.Max(so => so.Discount))
                    .Take(1)
                    .ToList()
            })
            .Take(count)
            .ToListAsync();

        return hotels;
    }

    public async Task<Hotel?> GetByIdAsync(Guid id, bool includeCity = false, bool includeRooms = false)
    {
        var hotelsQuery = _context.Hotels.AsQueryable();

        if (includeCity)
        {
            hotelsQuery = hotelsQuery.Include(h => h.City);
        }

        if (includeRooms)
        {
            hotelsQuery = hotelsQuery.Include(h => h.Rooms);
        }

        return await hotelsQuery.FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<Hotel?> GetByIdDetailsIncludedAsync(Guid id)
    {
        return await _context.Hotels
            .Include(h => h.City)
            .Include(h => h.Rooms)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<double> GetHotelRateAsync(Guid id)
    {
        return await _context.Hotels
            .Where(r => r.Id == id)
            .Select(r => r.Rating)
            .FirstOrDefaultAsync();
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

    public async Task UpdateRateAsync(Guid id, double rate)
    {
        var hotel = await _context.Hotels.FindAsync(id);

        if (hotel == null)
        {
            return;
        }

        hotel.Rating = rate;
    }

    public async Task<bool> ExistsAsync(Expression<Func<Hotel, bool>> predicate)
    {
        return await _context.Hotels.AnyAsync(predicate);
    }
}