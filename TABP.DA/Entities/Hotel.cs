using TABP.DAL.Interfaces;

namespace TABP.DAL.Entities;

public class Hotel : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid CityId { get; set; }
    public string Name { get; set; } = "";
    public string Owner { get; set; } = "";
    public string Address { get; set; } = "";
    public string? Description { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
    public ICollection<Image> Images { get; set; } = new List<Image>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public City City { get; set; }
}