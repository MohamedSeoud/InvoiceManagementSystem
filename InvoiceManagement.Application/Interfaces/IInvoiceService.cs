using InvoiceManagement.Application.DTOs;

namespace InvoiceManagement.Application.Interfaces;

public interface IInvoiceService
{
    Task<InvoiceDto> GetInvoiceByIdAsync(int id);
    Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();
    Task<InvoiceDto> CreateInvoiceAsync(InvoiceDto invoiceDto);
    Task UpdateInvoiceAsync(InvoiceDto invoiceDto);
    Task DeleteInvoiceAsync(int id);
}