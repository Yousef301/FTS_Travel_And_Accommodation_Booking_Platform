using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using TABP.Application.Extensions;
using TABP.Application.Helpers.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Queries.Hotels.GetHotelsForUser;

public class GetHotelsForUserQueryHandler : IRequestHandler<GetHotelsForUserQuery, PagedList<HotelUserResponse>>
{
    private readonly IHotelExpressions _hotelExpressions;
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelsForUserQueryHandler(IHotelRepository hotelRepository, IMapper mapper,
        IHotelExpressions hotelExpressions)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
        _hotelExpressions = hotelExpressions;
    }

    public async Task<PagedList<HotelUserResponse>> Handle(GetHotelsForUserQuery request,
        CancellationToken cancellationToken)
    {
        var hotels = await _hotelRepository.GetFilteredHotelsAsync(new Filters<Hotel>
        {
            Page = request.Page,
            PageSize = request.PageSize,
            FilterExpression = GetSearchExpression(request),
            SortExpression = _hotelExpressions.GetSortExpression(request.SortBy),
            SortOrder = request.SortOrder
        }, true, true);

        return _mapper.Map<PagedList<HotelUserResponse>>(hotels);
    }

    private Expression<Func<Hotel, bool>> GetSearchExpression(GetHotelsForUserQuery request)
    {
        return _hotelExpressions.GetHotelsBasedOnCityOrHotelNameExpression(request.SearchString)
            .And(_hotelExpressions.GetHotelsBasedOnAdultsAndChildrenExpression(request.NumberOfAdults,
                request.NumberOfChildren))
            .And(_hotelExpressions.GetHotelsBasedOnPriceRangeExpression(request.MinPrice, request.MaxPrice))
            .And(_hotelExpressions.GetHotelsBasedOnNumberOfAvailableRoomsAndDatesExpression(request.NumberOfRooms,
                request.CheckInDate,
                request.CheckOutDate))
            .And(_hotelExpressions.GetHotelsBasedOnReviewRatingExpression(request.ReviewRating))
            .And(_hotelExpressions.GetHotelsBasedOnAmenitiesExpression(request.Amenities));
    }
}