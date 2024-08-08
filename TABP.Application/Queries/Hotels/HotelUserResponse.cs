namespace TABP.Application.Queries.Hotels;

public class HotelUserResponse : HotelResponseBase
{
    public string City { get; set; }
    public string Owner { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public double Price { get; set; }
}