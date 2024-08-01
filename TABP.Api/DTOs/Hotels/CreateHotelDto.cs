namespace TABP.Web.DTOs.Hotels;

public class CreateHotelDto : HotelBase
{
    public Guid CityId { get; set; }
}