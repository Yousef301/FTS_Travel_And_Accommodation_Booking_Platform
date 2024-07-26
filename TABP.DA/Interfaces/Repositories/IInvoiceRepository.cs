using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IInvoiceRepository
{
    Task<IEnumerable<Invoice>> GetAsync();
    Task<Invoice?> GetByIdAsync(Guid id);
    Task<Invoice> CreateAsync(Invoice invoice);
    Task DeleteAsync(Invoice invoice);
    Task UpdateAsync(Invoice invoice);
    Task<bool> ExistsAsync(Expression<Func<Invoice, bool>> predicate);
}