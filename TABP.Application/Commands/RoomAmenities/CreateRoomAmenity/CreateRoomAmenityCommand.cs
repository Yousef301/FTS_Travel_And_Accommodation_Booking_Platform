using MediatR;

namespace TABP.Application.Commands.RoomAmenities.CreateRoomAmenity;

public class CreateRoomAmenityCommand : IRequest
{
    public Guid RoomId { get; set; }
    public IEnumerable<Guid> AmenitiesIds { get; set; }
}