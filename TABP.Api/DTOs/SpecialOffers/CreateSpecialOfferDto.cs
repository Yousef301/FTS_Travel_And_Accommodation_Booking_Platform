namespace TABP.Web.DTOs.SpecialOffers;

public class CreateSpecialOfferDto : SpecialOfferBase
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}