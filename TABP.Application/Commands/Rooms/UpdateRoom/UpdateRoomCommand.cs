using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace TABP.Application.Commands.Rooms.UpdateRoom;

public class UpdateRoomCommand : IRequest
{
    public Guid Id { get; set; }
    public JsonPatchDocument<RoomUpdate> RoomDocument { get; init; }
}