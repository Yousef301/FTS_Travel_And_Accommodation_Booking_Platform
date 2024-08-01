using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using TABP.Application.Commands.Amenities.CreateAmenity;
using TABP.Application.Commands.Amenities.UpdateAmenity;
using TABP.Web.DTOs.Amenities;

namespace TABP.Web.Profiles;

public class AmenityProfile : Profile
{
    public AmenityProfile()
    {
        CreateMap<CreateAmenityDto, CreateAmenityCommand>();
        CreateMap<JsonPatchDocument<UpdateAmenityDto>, JsonPatchDocument<AmenityUpdate>>();
        CreateMap<Operation<UpdateAmenityDto>, Operation<AmenityUpdate>>();
    }
}