using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.DbContexts;
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

    public async Task<bool> ExistsAsync(Expression<Func<Credential, bool>> predicate)
    {
        return await _context.Credentials.AnyAsync(predicate);
    }

    public Task<bool> UsernameExistsAsync(string username)
    {
        return _context.Credentials.AnyAsync(c => c.Username == username);
    }
}