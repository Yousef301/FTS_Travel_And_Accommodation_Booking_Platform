namespace TABP.Application.Queries.Hotels;

public class HotelWithFeaturedDealResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public double Rating { get; set; }
    public string OriginalPrice { get; set; }
    public string DiscountedPrice { get; set; }
}