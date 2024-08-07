namespace TABP.Application.Queries.Hotels;

public class HotelResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Owner { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public double Rating { get; set; }
    public double Price { get; set; }
}