using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface ICredentialRepository
{
    Task<IEnumerable<Credential>> GetAsync();
    Task<Credential?> GetByIdAsync(Guid id);
    Task<Credential?> GetByUsername(string username);
    Task<Credential> CreateAsync(Credential credential);
    Task DeleteAsync(Credential credential);
    Task UpdateAsync(Credential credential);
    Task<bool> ExistsAsync(Expression<Func<Credential, bool>> predicate);
}