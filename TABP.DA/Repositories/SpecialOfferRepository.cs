﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<SpecialOffer>> GetAsync()
    {
        return await _context.SpecialOffers.ToListAsync();
    }

    public async Task<SpecialOffer?> GetByIdAsync(Guid id)
    {
        return await _context.SpecialOffers.FindAsync(id);
    }

    public async Task<SpecialOffer> CreateAsync(SpecialOffer specialOffer)
    {
        var createdSpecialOffer = await _context.SpecialOffers
            .AddAsync(specialOffer);

        return createdSpecialOffer.Entity;
    }

    public async Task DeleteAsync(SpecialOffer specialOffer)
    {
        if (!await _context.SpecialOffers.AnyAsync(so => so.Id == specialOffer.Id))
        {
            return;
        }

        _context.SpecialOffers.Remove(specialOffer);
    }

    public async Task UpdateAsync(SpecialOffer specialOffer)
    {
        if (!await _context.SpecialOffers.AnyAsync(so => so.Id == specialOffer.Id))
            return;

        _context.SpecialOffers.Update(specialOffer);
    }

    public async Task<bool> ExistsAsync(Expression<Func<SpecialOffer, bool>> predicate)
    {
        return await _context.SpecialOffers.AnyAsync(predicate);
    }
}