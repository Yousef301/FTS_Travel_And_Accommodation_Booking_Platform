using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Hotels.GetHotels;

public class GetHotelsQueryHandler : IRequestHandler<GetHotelsQuery, IEnumerable<HotelResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;


    public GetHotelsQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<HotelResponse>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
    {
        var hotels = await _hotelRepository.GetIncludeCityAsync();

        return _mapper.Map<IEnumerable<HotelResponse>>(hotels);
    }
}