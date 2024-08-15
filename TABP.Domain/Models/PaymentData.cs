namespace TABP.Domain.Models;

public class PaymentData
{
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; }
    public string SuccessUrl { get; set; }
    public string CancelUrl { get; set; }
    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
}