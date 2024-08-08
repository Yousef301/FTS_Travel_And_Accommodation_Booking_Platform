namespace TABP.Application.Commands.Hotels.UpdateHotel;

public class HotelUpdate
{
    public Guid CityId { get; set; }
    public string Name { get; set; }
    public string Owner { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
}