using MediatR;
using TABP.Application.Queries.Amenities;

namespace TABP.Application.Commands.Amenities.CreateAmenity;

public class CreateAmenityCommand : IRequest<AmenityResponse>
{
    public string Name { get; init; }
}