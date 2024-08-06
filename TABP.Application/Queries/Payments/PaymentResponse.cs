namespace TABP.Application.Queries.Payments;

public class PaymentResponse
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentStatus { get; set; }
    public double TotalPrice { get; set; }
}