using TABP.DAL.Interfaces;

namespace TABP.DAL.Entities;

public class HotelImage : Image, IAuditableEntity
{
    public Guid HotelId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public Hotel Hotel { get; set; }
}