using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.DbContexts;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly TABPDbContext _context;

    public InvoiceRepository(TABPDbContext context)
    {
        _context = context;
    }

    public async Task<Invoice?> GetByBookingIdAsync(Guid id)
    {
        return await _context.Invoices
            .AsNoTracking()
            .Where(i => i.BookingId == id)
            .Include(i => i.Booking)
            .SingleOrDefaultAsync();
    }

    public async Task<Invoice> CreateAsync(Invoice invoice)
    {
        var createdInvoice = await _context.Invoices
            .AddAsync(invoice);

        return createdInvoice.Entity;
    }

    public async Task<bool> ExistsAsync(Expression<Func<Invoice, bool>> predicate)
    {
        return await _context.Invoices.AnyAsync(predicate);
    }
}