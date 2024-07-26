using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.DAL.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly TABPDbContext _context;

    public PaymentRepository(TABPDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Payment>> GetAsync()
    {
        return await _context.Payments.ToListAsync();
    }

    public async Task<Payment?> GetByIdAsync(Guid id)
    {
        return await _context.Payments.FindAsync(id);
    }

    public async Task<Payment> CreateAsync(Payment payment)
    {
        var createdPayment = await _context.Payments
            .AddAsync(payment);

        return createdPayment.Entity;
    }

    public async Task DeleteAsync(Payment payment)
    {
        if (!await _context.Payments.AnyAsync(p => p.Id == payment.Id))
        {
            return;
        }

        _context.Payments.Remove(payment);
    }

    public async Task UpdateAsync(Payment payment)
    {
        if (!await _context.Payments.AnyAsync(p => p.Id == payment.Id))
            return;

        _context.Payments.Update(payment);
    }

    public async Task<bool> ExistsAsync(Expression<Func<Payment, bool>> predicate)
    {
        return await _context.Payments.AnyAsync(predicate);
    }
}