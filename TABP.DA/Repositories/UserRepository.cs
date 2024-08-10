using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TABPDbContext _context;

    public UserRepository(TABPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> CreateAsync(User user)
    {
        var createdUser = await _context.Users
            .AddAsync(user);

        return createdUser.Entity;
    }

    public async Task DeleteAsync(User user)
    {
        if (!await _context.Users.AnyAsync(u => u.Id == user.Id))
        {
            return;
        }

        _context.Users.Remove(user);
    }

    public async Task UpdateAsync(User user)
    {
        if (!await _context.Users.AnyAsync(u => u.Id == user.Id))
            return;

        _context.Users.Update(user);
    }

    public async Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate)
    {
        return await _context.Users.AnyAsync(predicate);
    }
}