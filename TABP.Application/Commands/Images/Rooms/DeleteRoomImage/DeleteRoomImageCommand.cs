using MediatR;

namespace TABP.Application.Commands.Images.Rooms.DeleteRoomImage;

public class DeleteRoomImageCommand : IRequest
{
    public Guid RoomId { get; set; }
    public Guid ImageId { get; set; }
}