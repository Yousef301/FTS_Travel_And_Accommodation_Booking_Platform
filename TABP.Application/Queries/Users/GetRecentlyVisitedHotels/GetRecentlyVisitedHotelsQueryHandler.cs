using AutoMapper;
using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Users.GetRecentlyVisitedHotels;

public class GetRecentlyVisitedHotelsQueryHandler : IRequestHandler<GetRecentlyVisitedHotelsQuery,
    IEnumerable<RecentlyVisitedHotelsResponse>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetRecentlyVisitedHotelsQueryHandler(IBookingRepository bookingRepository,
        IHotelRepository hotelRepository, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RecentlyVisitedHotelsResponse>> Handle(GetRecentlyVisitedHotelsQuery request,
        CancellationToken cancellationToken)
    {
        var hotels = new List<Hotel>();

        var hotelsId = await _bookingRepository
            .GetHotelsIdsForAUserBookingsAsync(request.UserId);

        foreach (var hotelId in hotelsId)
        {
            var hotel = await _hotelRepository.GetByIdAsync(hotelId, true, true);
            hotels.Add(hotel);
        }

        return _mapper.Map<IEnumerable<RecentlyVisitedHotelsResponse>>(hotels);
    }
}