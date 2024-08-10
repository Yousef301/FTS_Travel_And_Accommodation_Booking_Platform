using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.DbContexts;
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

    public async Task<IEnumerable<Payment>> GetUserPaymentsAsync(Guid userId)
    {
        return await _context.Payments
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<Payment> CreateAsync(Payment payment)
    {
        var createdPayment = await _context.Payments
            .AddAsync(payment);

        return createdPayment.Entity;
    }

    public async Task<bool> ExistsAsync(Expression<Func<Payment, bool>> predicate)
    {
        return await _context.Payments.AnyAsync(predicate);
    }
}