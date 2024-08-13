using System.Data;
using Microsoft.EntityFrameworkCore;
using TABP.DAL.DbContexts;
using TABP.DAL.Interfaces;

namespace TABP.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly TABPDbContext _context;

    public UnitOfWork(TABPDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_context.Database.CurrentTransaction is null) return;

        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        if (_context.Database.CurrentTransaction is null) return;

        await _context.Database.RollbackTransactionAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}