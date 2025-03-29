namespace InvoiceManagement.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IInvoiceRepository InvoiceRepository { get; }
    Task<int> CompleteAsync();
}