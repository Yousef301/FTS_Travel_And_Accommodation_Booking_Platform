using TABP.DAL.Interfaces;

namespace TABP.DAL.Entities;

public class CityImage : Image, IAuditableEntity
{
    public Guid CityId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public City City { get; set; }
}