namespace TABP.Web.DTOs.Bookings;

public class BookingBase
{
    public Guid HotelId { get; set; }
    public List<Guid> RoomIds { get; set; }
    public DateOnly CheckInDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public DateOnly CheckOutDate { get; set; } = DateOnly.FromDateTime(DateTime.Today).AddDays(1);
    public string PaymentMethod { get; set; }
}