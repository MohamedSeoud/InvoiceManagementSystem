using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceManagement.Core.Entities;

public class InvoiceItem : BaseEntity
{
    public string Description { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [NotMapped]
    public decimal Amount => Quantity * UnitPrice;

    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; }
}