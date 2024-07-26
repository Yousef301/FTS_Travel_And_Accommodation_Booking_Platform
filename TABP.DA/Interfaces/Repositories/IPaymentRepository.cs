using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task<IEnumerable<Payment>> GetAsync();
    Task<Payment?> GetByIdAsync(Guid id);
    Task<Payment> CreateAsync(Payment payment);
    Task DeleteAsync(Payment payment);
    Task UpdateAsync(Payment payment);
    Task<bool> ExistsAsync(Expression<Func<Payment, bool>> predicate);
}