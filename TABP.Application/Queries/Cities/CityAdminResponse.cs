namespace TABP.Application.Queries.Cities;

public class CityAdminResponse : CityResponse
{
    public int NumberOfHotels { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}