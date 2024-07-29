using MediatR;

namespace TABP.Application.Queries.Amenities.GetAmenityById;

public class GetAmenityByIdQuery : IRequest<AmenityResponse>
{
    public Guid Id { get; set; }
}