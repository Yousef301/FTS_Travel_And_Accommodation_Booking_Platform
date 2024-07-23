using TABP.DAL.Interfaces;

namespace TABP.DAL.Entities;

public class Image : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid? RoomId { get; set; }
    public Guid? CityId { get; set; }
    public Guid? HotelId { get; set; }
    public string ImagePath { get; set; } = "";
    public string? Description { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public Room? Room { get; set; }
    public City? City { get; set; }
    public Hotel? Hotel { get; set; }
}