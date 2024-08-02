using AutoMapper;
using TABP.Application.Commands.RoomAmenities.CreateRoomAmenity;
using TABP.Web.DTOs.RoomAmenities;

namespace TABP.Web.Profiles;

public class RoomAmenityProfile : Profile
{
    public RoomAmenityProfile()
    {
        CreateMap<CreateRoomAmenityDto, CreateRoomAmenityCommand>();
    }
}