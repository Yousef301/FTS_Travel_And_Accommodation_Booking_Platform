namespace TABP.Application.Queries.Hotels;

public class HotelWithFeaturedDealResponse : HotelResponseBase
{
    public string OriginalPrice { get; set; }
    public string DiscountedPrice { get; set; }
}