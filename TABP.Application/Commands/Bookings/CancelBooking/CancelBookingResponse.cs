namespace TABP.Application.Commands.Bookings.CancelBooking;

public class CancelBookingResponse
{
    public Guid BookingId { get; set; }
    public string BookingStatus { get; set; }
}