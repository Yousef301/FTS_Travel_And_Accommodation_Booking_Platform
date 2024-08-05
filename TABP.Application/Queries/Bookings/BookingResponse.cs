namespace TABP.Application.Queries.Bookings;

public class BookingResponse
{
    public Guid Id { get; set; }
    public string HotelName { get; set; }
    public DateOnly BookingDate { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public string BookingStatus { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public double TotalPrice { get; set; }
}