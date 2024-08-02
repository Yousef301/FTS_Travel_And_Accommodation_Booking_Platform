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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetHotelsWithFeaturedDealsQueryHandler(IHotelRepository hotelRepository, IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<HotelWithFeaturedDealResponse>> Handle(GetHotelsWithFeaturedDealsQuery request,
        CancellationToken cancellationToken)
    {
        var hotels = await _hotelRepository.GetHotelsWithDealsAsync();

        var hotelResponses = _mapper.Map<IEnumerable<HotelWithFeaturedDealResponse>>(hotels);

        return hotelResponses;
    }
}