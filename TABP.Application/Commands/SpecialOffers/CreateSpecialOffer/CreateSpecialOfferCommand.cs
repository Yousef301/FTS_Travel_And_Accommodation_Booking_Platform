using MediatR;
using TABP.Application.Queries.SpecialOffers;

namespace TABP.Application.Commands.SpecialOffers.CreateSpecialOffer;

public class CreateSpecialOfferCommand : IRequest
{
    public Guid RoomId { get; set; }
    public double Discount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}