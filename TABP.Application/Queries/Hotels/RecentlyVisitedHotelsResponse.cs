namespace TABP.Application.Queries.Hotels;

public class RecentlyVisitedHotelsResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public double Rating { get; set; }
    public double Price { get; set; }
    public string ThumbnailUrl { get; set; }
}