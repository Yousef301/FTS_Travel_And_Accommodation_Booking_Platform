using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using TABP.Application.Commands.Cities.CreateCity;
using TABP.Application.Commands.Cities.UpdateCity;
using TABP.Web.DTOs.Cities;

namespace TABP.Web.Profiles;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<CreateCityDto, CreateCityCommand>();
        CreateMap<JsonPatchDocument<UpdateCityDto>, JsonPatchDocument<CityUpdate>>();
        CreateMap<Operation<UpdateCityDto>, Operation<CityUpdate>>();
    }
}