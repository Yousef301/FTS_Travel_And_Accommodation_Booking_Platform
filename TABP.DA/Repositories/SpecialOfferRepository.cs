using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.DbContexts;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class SpecialOfferRepository : ISpecialOfferRepository
{
    private readonly TABPDbContext _context;

    public SpecialOfferRepository(TABPDbContext context)
    {
        _context = context;
    }

    public Task<SpecialOffer?> GetByIdAsync(Guid id)
    {
        return _context.SpecialOffers
            .SingleOrDefaultAsync(so => so.Id == id);
    }

    public async Task<IEnumerable<SpecialOffer>> GetExpiredOffersAsync()
    {
        return await _context.SpecialOffers
            .Where(so => so.IsActive && so.EndDate < DateTime.Now)
            .ToListAsync();
    }

    public async Task<IEnumerable<SpecialOffer>> GetRoomOffersAsync(Guid roomId)
    {
        return await _context.SpecialOffers
            .AsQueryable()
            .Where(so => so.RoomId == roomId && so.IsActive)
            .ToListAsync();
    }

    public async Task<SpecialOffer?> GetByRoomIdAndOfferIdAsync(Guid id, Guid roomId)
    {
        return await _context.SpecialOffers
            .SingleOrDefaultAsync(so => so.Id == id && so.RoomId == roomId);
    }

    public async Task<SpecialOffer> CreateAsync(SpecialOffer specialOffer)
    {
        var createdSpecialOffer = await _context.SpecialOffers
            .AddAsync(specialOffer);

        return createdSpecialOffer.Entity;
    }

    public void Delete(SpecialOffer specialOffer)
    {
        _context.SpecialOffers.Remove(specialOffer);
    }

    public void Update(SpecialOffer specialOffer)
    {
        _context.SpecialOffers.Update(specialOffer);
    }

    public async Task<bool> ExistsAsync(Expression<Func<SpecialOffer, bool>> predicate)
    {
        return await _context.SpecialOffers.AnyAsync(predicate);
    }
}