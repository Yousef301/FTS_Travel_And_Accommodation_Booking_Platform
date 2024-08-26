using TABP.DAL.Interfaces;

namespace TABP.DAL.Entities;

public class Amenity : IAuditableEntity
{
    public Guid Id { get; set; } = new Guid();
    public string Name { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public ICollection<RoomAmenity> RoomAmenities { get; set; } = new List<RoomAmenity>();
}