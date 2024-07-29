﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class CredentialRepository : ICredentialRepository
{
    private readonly TABPDbContext _context;

    public CredentialRepository(TABPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Credential>> GetAsync()
    {
        return await _context.Credentials.ToListAsync();
    }

    public async Task<Credential?> GetByIdAsync(Guid id)
    {
        return await _context.Credentials.FindAsync(id);
    }

    public async Task<Credential?> GetByUsername(string username)
    {
        return await _context.Credentials
            .Include(u => u.User)
            .FirstOrDefaultAsync(c => c.Username == username);
    }

    public async Task<Credential> CreateAsync(Credential credential)
    {
        var createdCredential = await _context.Credentials
            .AddAsync(credential);

        return createdCredential.Entity;
    }

    public async Task DeleteAsync(Credential credential)
    {
        if (!await _context.Credentials.AnyAsync(c => c.Id == credential.Id))
        {
            return;
        }

        _context.Credentials.Remove(credential);
    }

    public async Task UpdateAsync(Credential credential)
    {
        if (!await _context.Credentials.AnyAsync(c => c.Id == credential.Id))
            return;

        _context.Credentials.Update(credential);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Credential, bool>> predicate)
    {
        return await _context.Credentials.AnyAsync(predicate);
    }
}