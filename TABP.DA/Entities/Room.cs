using TABP.DAL.Enums;
using TABP.DAL.Interfaces;

namespace TABP.DAL.Entities;

public class Room : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public string RoomNumber { get; set; } = "";
    public RoomType RoomType { get; set; }
    public RoomStatus Status { get; set; } = RoomStatus.Available;
    public double Price { get; set; }
    public string? Description { get; set; } = "";
    public int MaxChildren { get; set; }
    public int MaxAdults { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public ICollection<Image> Images { get; set; } = new List<Image>();
    public ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();
    public ICollection<RoomAmenity> RoomAmenities { get; set; } = new List<RoomAmenity>();
    public ICollection<SpecialOffer> SpecialOffers { get; set; } = new List<SpecialOffer>();
    public Hotel Hotel { get; set; }
}