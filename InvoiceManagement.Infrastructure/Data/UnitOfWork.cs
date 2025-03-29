using InvoiceManagement.Core.Interfaces;
using InvoiceManagement.Infrastructure.Repositories;

namespace InvoiceManagement.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IInvoiceRepository _invoiceRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IInvoiceRepository InvoiceRepository =>
        _invoiceRepository ??= new InvoiceRepository(_context);

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
