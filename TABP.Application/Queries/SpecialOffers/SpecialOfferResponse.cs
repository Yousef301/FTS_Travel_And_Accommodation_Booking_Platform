namespace TABP.Application.Queries.SpecialOffers;

public class SpecialOfferResponse
{
    public Guid Id { get; set; }
    public double Discount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}