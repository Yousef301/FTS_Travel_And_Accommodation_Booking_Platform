using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using TABP.Application.Queries.SpecialOffers;

namespace TABP.Application.Commands.SpecialOffers.UpdateSpecialOffer;

public class UpdateSpecialOfferCommand : IRequest<SpecialOfferResponse>
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public JsonPatchDocument<SpecialOfferUpdate> SpecialOfferDocument { get; init; }
}