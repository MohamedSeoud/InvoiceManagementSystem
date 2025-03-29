using System.ComponentModel.DataAnnotations;

namespace InvoiceManagement.Application.DTOs;

public class InvoiceDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Invoice number is required")]
    [StringLength(50, ErrorMessage = "Invoice number cannot be longer than 50 characters")]
    public string InvoiceNumber { get; set; }

    [Required(ErrorMessage = "Date is required")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Customer name is required")]
    [StringLength(100, ErrorMessage = "Customer name cannot be longer than 100 characters")]
    public string CustomerName { get; set; }

    public decimal TotalAmount { get; set; }

    [Required(ErrorMessage = "At least one item is required")]
    [MinLength(1, ErrorMessage = "At least one item is required")]
    public List<InvoiceItemDto> Items { get; set; } = new List<InvoiceItemDto>();
}