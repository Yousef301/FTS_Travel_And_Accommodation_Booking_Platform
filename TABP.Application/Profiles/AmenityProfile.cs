using AutoMapper;
using TABP.Application.Commands.Amenities.CreateAmenity;
using TABP.Application.Commands.Amenities.UpdateAmenity;
using TABP.Application.Queries.Amenities;
using TABP.DAL.Entities;

namespace TABP.Application.Profiles;

public class AmenityProfile : Profile
{
    public AmenityProfile()
    {
        CreateMap<CreateAmenityCommand, Amenity>();
        CreateMap<Amenity, AmenityResponse>();
        CreateMap<Amenity, AmenityUpdate>();
        CreateMap<AmenityUpdate, Amenity>();
    }
}