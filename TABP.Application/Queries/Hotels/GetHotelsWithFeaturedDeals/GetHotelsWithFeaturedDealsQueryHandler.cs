using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Hotels.GetHotelsWithFeaturedDeals;

public class
    GetHotelsWithFeaturedDealsQueryHandler : IRequestHandler<GetHotelsWithFeaturedDealsQuery,
    IEnumerable<HotelWithFeaturedDealResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelsWithFeaturedDealsQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<HotelWithFeaturedDealResponse>> Handle(GetHotelsWithFeaturedDealsQuery request,
        CancellationToken cancellationToken)
    {
        var hotels = await _hotelRepository.GetHotelsWithDealsAsync(request.Count);

        var hotelResponses = _mapper.Map<IEnumerable<HotelWithFeaturedDealResponse>>(hotels);

        return hotelResponses;
    }
}