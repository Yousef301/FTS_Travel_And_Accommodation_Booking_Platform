using TABP.DAL.Entities;

namespace TABP.DAL.Models;

public class BookingDto : Booking
{
    public string HotelName { get; set; }
}