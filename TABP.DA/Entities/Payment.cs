using TABP.DAL.Interfaces;
using TABP.Shared.Enums;

namespace TABP.DAL.Entities;

public class Payment : IAuditableEntity
{
    public Guid Id { get; set; } = new Guid();
    public Guid UserId { get; set; }
    public Guid BookingId { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public decimal TotalPrice { get; set; }
    public User User { get; set; }
    public Booking Booking { get; set; }
}