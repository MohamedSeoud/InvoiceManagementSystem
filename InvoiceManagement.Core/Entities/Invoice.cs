using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceManagement.Core.Entities;

public class Invoice : BaseEntity
{
    public string InvoiceNumber { get; set; }
    public DateTime Date { get; set; }
    public string CustomerName { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; } 
    public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

    public void CalculateTotal()
    {
        TotalAmount = Items.Sum(item => item.Quantity * item.UnitPrice);
    }
}