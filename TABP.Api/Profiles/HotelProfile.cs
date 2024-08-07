using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using TABP.Application.Commands.Hotels.CreateHotel;
using TABP.Application.Commands.Hotels.UpdateHotel;
using TABP.Application.Queries.Hotels.GetHotels;
using TABP.Web.DTOs.Hotels;

namespace TABP.Web.Profiles;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<HotelsSearchDto, GetHotelsQuery>();
        CreateMap<CreateHotelDto, CreateHotelCommand>();
        CreateMap<JsonPatchDocument<UpdateHotelDto>, JsonPatchDocument<HotelUpdate>>();
        CreateMap<Operation<UpdateHotelDto>, Operation<HotelUpdate>>();
    }
}