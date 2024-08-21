using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.DbContexts;
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

    public async Task<PagedList<Hotel>> GetAsync(
        Filters<Hotel> filters,
        bool includeCity = false,
        bool includeRooms = false,
        bool includeThumbnail = false)
    {
        var hotelsQuery = _context.Hotels.AsQueryable();

        hotelsQuery = hotelsQuery.Where(filters.FilterExpression!);

        hotelsQuery = filters.SortOrder == SortOrder.DESC
            ? hotelsQuery.OrderByDescending(filters.SortExpression!)
            : hotelsQuery.OrderBy(filters.SortExpression!);

        if (includeCity)
        {
            hotelsQuery = hotelsQuery.Include(h => h.City);
        }

        if (includeRooms)
        {
            hotelsQuery = hotelsQuery.Include(h => h.Rooms);
        }

        if (includeThumbnail)
        {
            hotelsQuery = hotelsQuery.Include(c =>
                c.Images.Where(i => i.Thumbnail));
        }

        var hotels = await PagedList<Hotel>.CreateAsync(
            hotelsQuery,
            filters.Page,
            filters.PageSize
        );

        return hotels;
    }

    public async Task<PagedList<Hotel>> GetFilteredHotelsAsync(
        Filters<Hotel> filters,
        bool includeCity = false,
        bool includeRooms = false,
        bool includeThumbnail = false)
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

        if (includeThumbnail)
        {
            hotelsQuery = hotelsQuery.Include(c =>
                c.Images.Where(i => i.Thumbnail));
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

    public async Task<IEnumerable<FeaturedHotel>> GetHotelsWithDealsAsync(int count = 5)
    {
        var hotels = await _context.Hotels
            .Where(h => h.Rooms.Any(r => r.SpecialOffers.Any(so => so.IsActive)))
            .Select(h => new 
            {
                h.Id,
                h.CityId,
                h.Name,
                h.Owner,
                h.Address,
                h.Description,
                h.PhoneNumber,
                h.Email,
                h.Rating,
                ThumbnailPath = h.Images.SingleOrDefault(hi => hi.Thumbnail)!.ImagePath,
                BestRoomDeal = h.Rooms
                    .Where(r => r.SpecialOffers.Any(so => so.IsActive))
                    .Select(r => new
                    {
                        OriginalPrice = r.Price,
                        BestOffer = r.SpecialOffers
                            .Where(so => so.IsActive)
                            .Select(so => so.Discount)
                            .SingleOrDefault()
                    })
                    .FirstOrDefault()
            })
            .Take(count)
            .ToListAsync();

        return hotels.Select(h => new FeaturedHotel
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
            ThumbnailUrl = h.ThumbnailPath,
            OriginalPrice = h.BestRoomDeal.OriginalPrice,
            Discount = h.BestRoomDeal.BestOffer
        });
    }

    public async Task<Hotel?> GetByIdAsync(
        Guid id,
        bool includeCity = false,
        bool includeRooms = false,
        bool includeThumbnail = false)
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

        if (includeThumbnail)
        {
            hotelsQuery = hotelsQuery.Include(c =>
                c.Images.Where(i => i.Thumbnail));
        }

        return await hotelsQuery.SingleOrDefaultAsync(h => h.Id == id);
    }

    public async Task<double> GetHotelRateAsync(Guid id)
    {
        return await _context.Hotels
            .Where(r => r.Id == id)
            .Select(r => r.Rating)
            .SingleOrDefaultAsync();
    }

    public async Task<Hotel> CreateAsync(Hotel hotel)
    {
        var createdHotel = await _context.Hotels
            .AddAsync(hotel);

        return createdHotel.Entity;
    }

    public void Delete(Hotel hotel)
    {
        _context.Hotels.Remove(hotel);
    }

    public void Update(Hotel hotel)
    {
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