namespace TABP.Application.Queries.Hotels;

public class HotelResponseBase
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public double Rating { get; set; }
}