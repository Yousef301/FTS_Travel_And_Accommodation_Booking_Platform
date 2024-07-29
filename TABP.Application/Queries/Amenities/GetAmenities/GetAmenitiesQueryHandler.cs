using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Amenities.GetAmenities;

public class GetAmenitiesQueryHandler : IRequestHandler<GetAmenitiesQuery, IEnumerable<AmenityResponse>>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IMapper _mapper;

    public GetAmenitiesQueryHandler(IAmenityRepository amenityRepository, IMapper mapper)
    {
        _amenityRepository = amenityRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AmenityResponse>> Handle(GetAmenitiesQuery request,
        CancellationToken cancellationToken)
    {
        var amenities = await _amenityRepository.GetAsync();

        return _mapper.Map<IEnumerable<AmenityResponse>>(amenities);
    }
}