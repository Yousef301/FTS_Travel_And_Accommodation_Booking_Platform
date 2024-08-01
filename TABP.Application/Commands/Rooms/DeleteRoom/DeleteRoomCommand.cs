using MediatR;

namespace TABP.Application.Commands.Rooms.DeleteRoom;

public class DeleteRoomCommand : IRequest
{
    public Guid Id { get; set; }
}