using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces.Repositories;
using TABP.DAL.Models;
using TABP.Domain.Models;

namespace TABP.Application.Queries.Hotels.GetHotels;

public class GetHotelsQueryHandler : IRequestHandler<GetHotelsQuery, PagedList<HotelResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;


    public GetHotelsQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<HotelResponse>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
    {
        var hotels = await _hotelRepository.GetAsync(new Query
        {
            Page = request.Page,
            PageSize = request.PageSize
        }, true);

        return _mapper.Map<PagedList<HotelResponse>>(hotels);
    }
}