using AutoMapper;
using TABP.Application.Commands.Hotels.CreateHotel;
using TABP.Application.Commands.Hotels.UpdateHotel;
using TABP.Application.Queries.Hotels;
using TABP.DAL.Entities;
using TABP.DAL.Models;
using TABP.Domain.Models;

namespace TABP.Application.Profiles;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<CreateHotelCommand, Hotel>();
        CreateMap<Hotel, HotelUpdate>();
        CreateMap<HotelUpdate, Hotel>();
        CreateMap<Hotel, HotelResponseBase>();
        CreateMap<Hotel, HotelAdminResponse>()
            .ForMember(dest => dest.City,
                opt =>
                    opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.NumberOfRooms,
                opt =>
                    opt.MapFrom(src => src.Rooms.Count));
        ;
        CreateMap<Hotel, HotelUserResponse>()
            .ForMember(dest => dest.City,
                opt =>
                    opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.Price,
                opt =>
                    opt.MapFrom(src => src.Rooms.Min(r => r.Price)));

        CreateMap<PagedList<Hotel>, PagedList<HotelAdminResponse>>()
            .ForMember(dest => dest.Items,
                opt =>
                    opt.MapFrom(src => src.Items));

        CreateMap<PagedList<Hotel>, PagedList<HotelUserResponse>>()
            .ForMember(dest => dest.Items,
                opt =>
                    opt.MapFrom(src => src.Items));

        CreateMap<Hotel, RecentlyVisitedHotelsResponse>()
            .ForMember(dest => dest.City,
                opt =>
                    opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.Price,
                opt =>
                    opt.MapFrom(src => src.Rooms.Min(r => r.Price)));

        CreateMap<FeaturedHotel, HotelWithFeaturedDealResponse>()
            .ForMember(dest => dest.DiscountedPrice,
                opt =>
                    opt.MapFrom(src =>
                        src.OriginalPrice - Convert.ToDecimal(src.Discount) * src.OriginalPrice / 100));
    }
}