using TABP.DAL.Interfaces;
using TABP.Domain.Enums;

namespace TABP.DAL.Entities;

public class Invoice : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public double TotalPrice { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public Booking Booking { get; set; }
}