using AutoMapper;
using TABP.Application.Queries.Cities.GetCitiesForAdmin;
using TABP.Application.Queries.Hotels.GetHotelsForAdmin;
using TABP.Application.Queries.Rooms.GetRoomsForAdmin;
using TABP.Web.DTOs;

namespace TABP.Web.Profiles;

public class FilterParametersProfile : Profile
{
    public FilterParametersProfile()
    {
        CreateMap<FilterParameters, GetCitiesForAdminQuery>();
        CreateMap<FilterParameters, GetHotelsForAdminQuery>();
        CreateMap<FilterParameters, GetRoomsForAdminQuery>();
    }
}