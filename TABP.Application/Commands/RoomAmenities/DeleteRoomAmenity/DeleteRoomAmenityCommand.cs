using MediatR;

namespace TABP.Application.Commands.RoomAmenities.DeleteRoomAmenity;

public class DeleteRoomAmenityCommand : IRequest
{
    public Guid Id { get; set; }
}