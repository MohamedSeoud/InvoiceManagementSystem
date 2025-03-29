using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InvoiceManagement.Application.DTOs;
using InvoiceManagement.Application.Interfaces;

namespace InvoiceManagement.Web.Pages.Invoices
{
    public class DeleteModel : PageModel
    {
        private readonly IInvoiceService _invoiceService;

        public DeleteModel(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [BindProperty]
        public InvoiceDto Invoice { get; set; } = new InvoiceDto();

        public async Task<IActionResult> OnGetAsync([FromQuery] int id)
        {
            int Id = Convert.ToInt16(id);
            Invoice = await _invoiceService.GetInvoiceByIdAsync(Id);

            if (Invoice == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Invoice.Id == 0)
            {
                return BadRequest();
            }

            await _invoiceService.DeleteInvoiceAsync(Invoice.Id);
            return RedirectToPage("/Invoices/Index");
        }
    }
}
