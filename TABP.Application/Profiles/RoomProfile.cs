using AutoMapper;
using TABP.Application.Commands.Rooms.CreateRoom;
using TABP.Application.Commands.Rooms.UpdateRoom;
using TABP.Application.Queries.Rooms;
using TABP.DAL.Entities;
using TABP.Shared.Models;

namespace TABP.Application.Profiles;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<Room, RoomUpdate>();
        CreateMap<RoomUpdate, Room>();
        CreateMap<CreateRoomCommand, Room>();
        CreateMap<PagedList<Room>, PagedList<RoomAdminResponse>>();
        CreateMap<Room, RoomAdminResponse>()
            .ForMember(dest => dest.Amenities
                , opt => opt.MapFrom(
                    src => src.RoomAmenities.Select(ra => ra.Amenity)))
            .ForMember(dest => dest.RoomType
                , opt => opt.MapFrom(
                    src => src.RoomType.ToString()))
            .ForMember(dest => dest.Status
                , opt => opt.MapFrom(
                    src => src.Status.ToString()));
        
        CreateMap<Room, RoomResponse>()
            .ForMember(dest => dest.Amenities
                , opt => opt.MapFrom(
                    src => src.RoomAmenities.Select(ra => ra.Amenity)))
            .ForMember(dest => dest.RoomType
                , opt => opt.MapFrom(
                    src => src.RoomType.ToString()))
            .ForMember(dest => dest.Status
                , opt => opt.MapFrom(
                    src => src.Status.ToString()));
    }
}