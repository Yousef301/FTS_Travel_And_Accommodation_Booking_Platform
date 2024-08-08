namespace TABP.Web.DTOs.Hotels;

public class HotelBase
{
    public Guid CityId { get; set; }
    public string Name { get; set; }
    public string Owner { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
}