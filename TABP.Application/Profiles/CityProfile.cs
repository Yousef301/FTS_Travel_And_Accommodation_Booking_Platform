using AutoMapper;
using TABP.Application.Commands.Cities.CreateCity;
using TABP.Application.Commands.Cities.UpdateCity;
using TABP.Application.Queries.Cities;
using TABP.DAL.Entities;
using TABP.DAL.Models.Procedures;
using TABP.Shared.Models;

namespace TABP.Application.Profiles;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<CreateCityCommand, City>();
        CreateMap<City, CityResponse>();
        CreateMap<City, CityAdminResponse>()
            .ForMember(dest => dest.NumberOfHotels,
                opt =>
                    opt.MapFrom(src => src.Hotels.Count));
        CreateMap<PagedList<City>, PagedList<CityAdminResponse>>();
        CreateMap<City, CityUpdate>();
        CreateMap<CityUpdate, City>();
        CreateMap<TrendingCities, TrendingCitiesResponse>();
    }
}