using AutoMapper;
using TABP.DAL.Entities;
using TABP.Application.Queries.Users;

namespace TABP.Application.Profiles;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<Hotel, RecentlyVisitedHotelsResponse>()
            .ForMember(dest => dest.City,
                opt =>
                    opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.Price,
                opt =>
                    opt.MapFrom(src => src.Rooms.Min(r => r.Price)));
    }
}