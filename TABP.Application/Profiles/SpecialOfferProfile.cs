using AutoMapper;
using TABP.Application.Commands.SpecialOffers.CreateSpecialOffer;
using TABP.Application.Commands.SpecialOffers.UpdateSpecialOffer;
using TABP.Application.Queries.SpecialOffers;
using TABP.DAL.Entities;

namespace TABP.Application.Profiles;

public class SpecialOfferProfile : Profile
{
    public SpecialOfferProfile()
    {
        CreateMap<SpecialOffer, SpecialOfferResponse>();
        CreateMap<CreateSpecialOfferCommand, SpecialOffer>();
        CreateMap<SpecialOffer, SpecialOfferUpdate>();
        CreateMap<SpecialOfferUpdate, SpecialOffer>();
    }
}