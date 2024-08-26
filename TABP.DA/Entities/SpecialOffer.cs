using TABP.DAL.Interfaces;

namespace TABP.DAL.Entities;

public class SpecialOffer : IAuditableEntity
{
    public Guid Id { get; set; } = new Guid();
    public Guid RoomId { get; set; }
    public double Discount { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public Room Room { get; set; }
}