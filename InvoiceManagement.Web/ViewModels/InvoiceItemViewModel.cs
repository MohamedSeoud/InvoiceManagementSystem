namespace InvoiceManagement.Web.ViewModels;

public class InvoiceItemViewModel
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
