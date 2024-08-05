using TABP.Web.Enums;

namespace TABP.Web.DTOs.Bookings;

public class BookingBase
{
    public Guid HotelId { get; set; }
    public List<Guid> RoomIds { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public string PaymentMethod { get; set; }
}