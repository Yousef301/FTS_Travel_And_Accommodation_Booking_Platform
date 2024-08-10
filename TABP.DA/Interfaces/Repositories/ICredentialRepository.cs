using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface ICredentialRepository
{
    Task<Credential?> GetByUsername(string username);
    Task<Credential> CreateAsync(Credential credential);
    Task<bool> ExistsAsync(Expression<Func<Credential, bool>> predicate);
    Task<bool> UsernameExistsAsync(string username);
}