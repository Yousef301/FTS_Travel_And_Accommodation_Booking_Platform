using System.Collections;
using MediatR;

namespace TABP.Application.Queries.Amenities.GetAmenities;

public class GetAmenitiesQuery : IRequest<IEnumerable<AmenityResponse>>
{
}