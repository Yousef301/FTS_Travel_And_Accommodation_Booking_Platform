using MediatR;

namespace TABP.Application.Commands.Rooms.DeleteRoom;

public class DeleteRoomCommand : IRequest
{
    public Guid HotelId { get; set; }
    public Guid Id { get; set; }
}