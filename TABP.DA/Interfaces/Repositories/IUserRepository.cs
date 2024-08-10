using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate);
}