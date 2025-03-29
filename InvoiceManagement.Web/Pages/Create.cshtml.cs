using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InvoiceManagement.Application.DTOs;
using InvoiceManagement.Application.Interfaces;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace InvoiceManagement.Web.Pages.Invoices;

public class CreateModel : PageModel
{
    private readonly IInvoiceService _invoiceService;

    public CreateModel(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [BindProperty]
    public InvoiceDto Invoice { get; set; } = new InvoiceDto
    {
        Items = new List<InvoiceItemDto> { new InvoiceItemDto() }
    };

    public void OnGet()
    {
        // Ensure we always have at least one item
        if (Invoice.Items == null || Invoice.Items.Count == 0)
        {
            Invoice.Items = new List<InvoiceItemDto> { new InvoiceItemDto() };
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {

        if (Invoice?.Items != null)
        {
            foreach (var item in Invoice.Items)
            {
                Debug.WriteLine($"Item - Desc: {item.Description}, Qty: {item.Quantity}, Price: {item.UnitPrice}");
            }
        }

        // Ensure items collection exists
        Invoice.Items ??= new List<InvoiceItemDto>();

        // Remove empty items (where description is null or empty)
        Invoice.Items = Invoice.Items
            .Where(item => !string.IsNullOrWhiteSpace(item.Description))
            .ToList();

        // If no valid items left, add one empty item
        if (Invoice.Items.Count == 0)
        {
            Invoice.Items.Add(new InvoiceItemDto());
            ModelState.AddModelError("Invoice.Items", "At least one item is required");
        }

        // Calculate amounts for each item
        foreach (var item in Invoice.Items)
        {
            item.Amount = item.Quantity * item.UnitPrice;
        }

        // Calculate total amount
        Invoice.TotalAmount = Invoice.Items.Sum(i => i.Amount);

        // Manual validation for items
        bool hasValidItems = false;
        foreach (var item in Invoice.Items)
        {
            if (!string.IsNullOrWhiteSpace(item.Description) && item.Quantity > 0 && item.UnitPrice > 0)
            {
                hasValidItems = true;
                break;
            }
        }

        if (!hasValidItems)
        {
            ModelState.AddModelError("Invoice.Items", "At least one valid item is required");
        }

        if (!ModelState.IsValid)
        {
            // Debug: Log ModelState errors
            foreach (var key in ModelState.Keys)
            {
                var entry = ModelState[key];
                if (entry.Errors.Count > 0)
                {
                    Debug.WriteLine($"Key: {key}, Errors: {string.Join(", ", entry.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            // Ensure we have at least one item for the form
            if (Invoice.Items == null || Invoice.Items.Count == 0)
            {
                Invoice.Items = new List<InvoiceItemDto> { new InvoiceItemDto() };
            }

            return Page();
        }

        try
        {
            await _invoiceService.CreateInvoiceAsync(Invoice);
            return RedirectToPage("/Invoices/Index");
        }
        catch (FluentValidation.ValidationException ex)
        {
            foreach (var error in ex.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            // Ensure items collection is properly initialized for the view
            if (Invoice.Items == null || Invoice.Items.Count == 0)
            {
                Invoice.Items = new List<InvoiceItemDto> { new InvoiceItemDto() };
            }

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while saving the invoice: " + ex.Message);

            // Ensure items collection is properly initialized for the view
            if (Invoice.Items == null || Invoice.Items.Count == 0)
            {
                Invoice.Items = new List<InvoiceItemDto> { new InvoiceItemDto() };
            }

            return Page();
        }
    }
}