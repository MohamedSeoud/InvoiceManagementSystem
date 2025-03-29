namespace InvoiceManagement.Web.ViewModels;

public class InvoiceViewModel
{
    public int Id { get; set; }
    public int InvoiceNumber { get; set; }
    public DateTime Date { get; set; }
    public string CustomerName { get; set; }
    public List<InvoiceItemViewModel> Items { get; set; } = new List<InvoiceItemViewModel>();
    public decimal TotalAmount => Items.Sum(i => i.Quantity * i.UnitPrice);
}
