using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace TABP.Application.Commands.Amenities.UpdateAmenity;

public class UpdateAmenityCommand : IRequest
{
    public Guid Id { get; init; }
    public JsonPatchDocument<AmenityUpdate> AmenityDocument { get; init; }
}