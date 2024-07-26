using TABP.DAL.Interfaces;

namespace TABP.DAL.Entities;

public class RoomImage : Image, IAuditableEntity
{
    public Guid RoomId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public Room Room { get; set; }
}