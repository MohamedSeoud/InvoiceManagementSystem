using InvoiceManagement.Core.Entities;
using InvoiceManagement.Core.Interfaces;
using InvoiceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagement.Infrastructure.Repositories;

public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Invoice> GetInvoiceWithItemsAsync(int id)
    {
        return await _context.Invoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == id);
    }
}

