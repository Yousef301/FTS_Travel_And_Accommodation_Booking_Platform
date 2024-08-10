using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.DbContexts;
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

    public async Task<User> CreateAsync(User user)
    {
        var createdUser = await _context.Users
            .AddAsync(user);

        return createdUser.Entity;
    }

    public async Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate)
    {
        return await _context.Users.AnyAsync(predicate);
    }
}