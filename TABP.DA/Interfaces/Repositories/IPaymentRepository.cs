using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task<IEnumerable<Payment>> GetUserPaymentsAsync(Guid userId);
    Task<Payment> CreateAsync(Payment payment);
    Task<bool> ExistsAsync(Expression<Func<Payment, bool>> predicate);
}