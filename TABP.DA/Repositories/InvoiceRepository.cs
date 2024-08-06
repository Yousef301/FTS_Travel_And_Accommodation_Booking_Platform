using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<Invoice>> GetAsync()
    {
        return await _context.Invoices.ToListAsync();
    }

    public async Task<Invoice?> GetByIdAsync(Guid id)
    {
        return await _context.Invoices.FindAsync(id);
    }

    public async Task<Invoice?> GetByBookingIdAsync(Guid id)
    {
        return await _context.Invoices
            .Where(i => i.BookingId == id)
            .Include(i => i.Booking)
            .FirstOrDefaultAsync();
    }

    public async Task<Invoice> CreateAsync(Invoice invoice)
    {
        var createdInvoice = await _context.Invoices
            .AddAsync(invoice);

        return createdInvoice.Entity;
    }

    public async Task DeleteAsync(Invoice invoice)
    {
        if (!await _context.Invoices.AnyAsync(i => i.Id == invoice.Id))
        {
            return;
        }

        _context.Invoices.Remove(invoice);
    }

    public async Task UpdateAsync(Invoice invoice)
    {
        if (!await _context.Invoices.AnyAsync(i => i.Id == invoice.Id))
            return;

        _context.Invoices.Update(invoice);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Invoice, bool>> predicate)
    {
        return await _context.Invoices.AnyAsync(predicate);
    }
}