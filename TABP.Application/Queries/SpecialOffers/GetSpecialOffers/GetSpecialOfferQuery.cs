using MediatR;

namespace TABP.Application.Queries.SpecialOffers.GetSpecialOffers;

public class GetSpecialOfferQuery : IRequest<IEnumerable<SpecialOfferResponse>>
{
    public Guid RoomId { get; set; }
}