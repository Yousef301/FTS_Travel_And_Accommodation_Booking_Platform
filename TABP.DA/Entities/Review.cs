using TABP.DAL.Interfaces;

namespace TABP.DAL.Entities;

public class Review : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid HotelId { get; set; }
    public double Rate { get; set; }
    public string Comment { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public User User { get; set; }
    public Hotel Hotel { get; set; }
}