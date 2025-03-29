using InvoiceManagement.Core.Entities;

namespace InvoiceManagement.Core.Interfaces;

public interface IInvoiceRepository : IRepository<Invoice>
{
    Task<Invoice> GetInvoiceWithItemsAsync(int id);
}
