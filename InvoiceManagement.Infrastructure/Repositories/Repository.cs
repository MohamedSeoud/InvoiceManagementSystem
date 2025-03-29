
using InvoiceManagement.Core.Entities;
using InvoiceManagement.Core.Interfaces;
using InvoiceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InvoiceManagement.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _entities;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _entities.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _entities.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _entities.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _entities.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _entities.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _entities.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _entities.AnyAsync(e => e.Id == id);
    }
}