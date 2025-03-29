using System.ComponentModel.DataAnnotations;

namespace InvoiceManagement.Application.DTOs;

public class InvoiceItemDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; } = 1;

    [Required(ErrorMessage = "Unit price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be at least 0.01")]
    public decimal UnitPrice { get; set; } = 0.01m;

    public decimal Amount { get; set; }
}