using TABP.DAL.Interfaces;

namespace TABP.DAL.Entities;

public class Invoice : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public double TotalPrice { get; set; }
    public DateTime InvoiceDate { get; set; }
    public bool IsPaid { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public Booking Booking { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}