using TABP.DAL.Enums;
using TABP.DAL.Interfaces;

namespace TABP.DAL.Entities;

public class Payment : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid InvoiceId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public double TotalPrice { get; set; }
    public User User { get; set; }
    public Invoice Invoice { get; set; }
}