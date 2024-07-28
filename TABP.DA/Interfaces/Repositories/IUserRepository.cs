using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User> CreateAsync(User user);
    Task DeleteAsync(User user);
    Task UpdateAsync(User user);
    Task<bool> EmailExist(string email);
    Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate);
}