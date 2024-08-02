using MediatR;

namespace TABP.Application.Queries.RoomAmenities.GetRoomAmenities;

public class GetRoomAmenitiesQuery : IRequest<IEnumerable<RoomAmenityResponse>>
{
    public Guid RoomId { get; set; }
}