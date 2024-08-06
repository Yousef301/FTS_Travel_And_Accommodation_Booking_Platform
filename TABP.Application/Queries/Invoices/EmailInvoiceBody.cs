namespace TABP.Application.Queries.Invoices;

public class EmailInvoiceBody
{
    public Guid InvoiceId { get; set; }
    public Guid BookingId { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime InvoiceDate { get; set; }
    public double TotalAmount { get; set; }
}