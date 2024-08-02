using AutoMapper;
using TABP.Application.Commands.RoomAmenities.CreateRoomAmenity;
using TABP.Application.Queries.RoomAmenities;
using TABP.DAL.Entities;

namespace TABP.Application.Profiles;

public class RoomAmenityProfile : Profile
{
    public RoomAmenityProfile()
    {
        CreateMap<CreateRoomAmenityCommand, RoomAmenity>();
        CreateMap<RoomAmenity, RoomAmenityResponse>()
            .ForMember(dest => dest.Name,
                opt =>
                    opt.MapFrom(src => src.Amenity.Name));
    }
}