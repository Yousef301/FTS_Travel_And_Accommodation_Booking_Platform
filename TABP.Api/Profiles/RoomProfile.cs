using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using TABP.Application.Commands.Rooms.CreateRoom;
using TABP.Application.Commands.Rooms.UpdateRoom;
using TABP.Application.Queries.Rooms.GetAvailableRooms;
using TABP.Web.DTOs.Rooms;

namespace TABP.Web.Profiles;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<CreateRoomDto, CreateRoomCommand>();
        CreateMap<GetAvailableRoomsDto, GetAvailableRoomsQuery>();
        CreateMap<JsonPatchDocument<UpdateRoomDto>, JsonPatchDocument<RoomUpdate>>();
        CreateMap<Operation<UpdateRoomDto>, Operation<RoomUpdate>>();
    }
}