using TABP.DAL.Interfaces;
using TABP.Shared.Enums;

namespace TABP.DAL.Entities;

public class Room : IAuditableEntity
{
    public Guid Id { get; set; } = new Guid();
    public Guid HotelId { get; set; }
    public string RoomNumber { get; set; } = "";
    public RoomType RoomType { get; set; }
    public RoomStatus Status { get; set; } = RoomStatus.Available;
    public decimal Price { get; set; }
    public string? Description { get; set; } = "";
    public int MaxChildren { get; set; }
    public int MaxAdults { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public ICollection<RoomImage> Images { get; set; } = new List<RoomImage>();
    public ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();
    public ICollection<RoomAmenity> RoomAmenities { get; set; } = new List<RoomAmenity>();
    public ICollection<SpecialOffer> SpecialOffers { get; set; } = new List<SpecialOffer>();
    public Hotel Hotel { get; set; }
}