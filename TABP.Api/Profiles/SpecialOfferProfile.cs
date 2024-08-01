using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using TABP.Application.Commands.SpecialOffers.CreateSpecialOffer;
using TABP.Application.Commands.SpecialOffers.UpdateSpecialOffer;
using TABP.Web.DTOs.SpecialOffers;

namespace TABP.Web.Profiles;

public class SpecialOfferProfile : Profile
{
    public SpecialOfferProfile()
    {
        CreateMap<CreateSpecialOfferDto, CreateSpecialOfferCommand>();
        CreateMap<JsonPatchDocument<UpdateSpecialOfferDto>, JsonPatchDocument<SpecialOfferUpdate>>();
        CreateMap<Operation<UpdateSpecialOfferDto>, Operation<SpecialOfferUpdate>>();
    }
}