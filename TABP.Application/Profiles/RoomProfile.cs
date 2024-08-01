using AutoMapper;
using TABP.Application.Commands.Rooms.CreateRoom;
using TABP.Application.Commands.Rooms.UpdateRoom;
using TABP.Application.Queries.Rooms;
using TABP.DAL.Entities;

namespace TABP.Application.Profiles;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<Room, RoomUpdate>();
        CreateMap<RoomUpdate, Room>();
        CreateMap<CreateRoomCommand, Room>();
        CreateMap<Room, RoomResponse>()
            .ForMember(dest => dest.Amenities
                , opt => opt.MapFrom(
                    src => src.RoomAmenities.Select(ra => ra.Amenity)));
    }
}