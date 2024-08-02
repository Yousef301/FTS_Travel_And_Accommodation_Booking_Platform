using MediatR;

namespace TABP.Application.Commands.RoomAmenities.CreateRoomAmenity;

public class CreateRoomAmenityCommand : IRequest
{
    public Guid RoomId { get; set; }
    public Guid AmenityId { get; set; }
}