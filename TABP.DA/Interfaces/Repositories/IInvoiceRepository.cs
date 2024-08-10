using System.Linq.Expressions;
using TABP.DAL.Entities;

namespace TABP.DAL.Interfaces.Repositories;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByBookingIdAsync(Guid id);
    Task<Invoice> CreateAsync(Invoice invoice);
    Task<bool> ExistsAsync(Expression<Func<Invoice, bool>> predicate);
}