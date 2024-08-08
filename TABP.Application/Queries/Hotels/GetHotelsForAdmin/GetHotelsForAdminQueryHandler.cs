using AutoMapper;
using MediatR;
using TABP.Application.Helpers.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Queries.Hotels.GetHotelsForAdmin;

public class GetHotelsForAdminQueryHandler : IRequestHandler<GetHotelsForAdminQuery, PagedList<HotelAdminResponse>>
{
    private readonly IHotelExpressions _hotelExpressions;
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelsForAdminQueryHandler(IHotelRepository hotelRepository, IMapper mapper,
        IHotelExpressions hotelExpressions)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
        _hotelExpressions = hotelExpressions;
    }

    public async Task<PagedList<HotelAdminResponse>> Handle(GetHotelsForAdminQuery request,
        CancellationToken cancellationToken)
    {
        var hotels = await _hotelRepository.GetAsync(new Filters<Hotel>
        {
            FilterExpression = _hotelExpressions.GetHotelsBasedOnCityOrHotelNameExpression(request.SearchString),
            SortExpression = _hotelExpressions.GetSortExpression(request.SortBy),
            SortOrder = request.SortOrder,
            Page = request.Page,
            PageSize = request.PageSize
        }, true, true);

        return _mapper.Map<PagedList<HotelAdminResponse>>(hotels);
    }
}