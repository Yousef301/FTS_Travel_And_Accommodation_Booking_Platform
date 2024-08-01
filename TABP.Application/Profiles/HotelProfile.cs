using AutoMapper;
using TABP.Application.Commands.Hotels.CreateHotel;
using TABP.Application.Commands.Hotels.UpdateHotel;
using TABP.Application.Queries.Hotels;
using TABP.DAL.Entities;
using TABP.Application.Queries.Users;

namespace TABP.Application.Profiles;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<Hotel, HotelResponse>()
            .ForMember(dest => dest.City,
                opt =>
                    opt.MapFrom(src => src.City.Name));
        CreateMap<Hotel, RecentlyVisitedHotelsResponse>()
            .ForMember(dest => dest.City,
                opt =>
                    opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.Price,
                opt =>
                    opt.MapFrom(src => src.Rooms.Min(r => r.Price)));
        CreateMap<CreateHotelCommand, Hotel>();
        CreateMap<Hotel, HotelUpdate>();
        CreateMap<HotelUpdate, Hotel>();
    }
}