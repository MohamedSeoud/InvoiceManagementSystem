// Pages/Invoices/Index.cshtml.cs
using InvoiceManagement.Application.DTOs;
using InvoiceManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoiceManagement.Web.Pages.Invoices
{
    public class IndexModel : PageModel
    {
        private readonly IInvoiceService _invoiceService;

        public IndexModel(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        public IList<InvoiceDto> Invoices { get; set; }

        public async Task OnGetAsync()
        {
            Invoices = (await _invoiceService.GetAllInvoicesAsync()) as IList<InvoiceDto>;
        }
    }
}