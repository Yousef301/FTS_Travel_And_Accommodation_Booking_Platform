using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using TABP.Application.Queries.Amenities;

namespace TABP.Application.Commands.Amenities.UpdateAmenity;

public class UpdateAmenityCommand : IRequest<AmenityResponse>
{
    public Guid Id { get; init; }
    public JsonPatchDocument<AmenityUpdate> AmenityDocument { get; init; }
}