using TABP.DAL.Interfaces;

namespace TABP.DAL.Entities;

public class RoomAmenity : IAuditableEntity
{
    public Guid Id { get; set; } = new Guid();
    public Guid RoomId { get; set; }
    public Guid AmenityId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public Room Room { get; set; }
    public Amenity Amenity { get; set; }
}