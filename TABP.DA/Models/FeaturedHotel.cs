using TABP.DAL.Entities;

namespace TABP.DAL.Models;

public class FeaturedHotel
{
    public Guid Id { get; set; }
    public Guid CityId { get; set; }
    public string Name { get; set; }
    public string Owner { get; set; }
    public string Address { get; set; }
    public string? Description { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public double Rating { get; set; }
    public decimal OriginalPrice { get; set; }
    public double Discount { get; set; }
    public string ThumbnailUrl { get; set; }
}