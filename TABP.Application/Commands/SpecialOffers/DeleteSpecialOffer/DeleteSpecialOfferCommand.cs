using MediatR;

namespace TABP.Application.Commands.SpecialOffers.DeleteSpecialOffer;

public class DeleteSpecialOfferCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
}