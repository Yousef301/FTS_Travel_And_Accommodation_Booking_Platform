using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using TABP.Application.Extensions;
using TABP.Application.Helpers.Interfaces;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Queries.Hotels.GetHotelsForUser;

public class GetHotelsForUserQueryHandler : IRequestHandler<GetHotelsForUserQuery, PagedList<HotelUserResponse>>
{
    private readonly IHotelExpressions _hotelExpressions;
    private readonly IHotelRepository _hotelRepository;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    public GetHotelsForUserQueryHandler(IHotelRepository hotelRepository,
        IMapper mapper,
        IHotelExpressions hotelExpressions,
        IImageService imageService)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
        _hotelExpressions = hotelExpressions;
        _imageService = imageService;
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
        }, true, true, true);

        var mappedHotels = _mapper.Map<PagedList<HotelUserResponse>>(hotels);

        var thumbnailPaths = mappedHotels.Items
            .Where(hotel => hotel.ThumbnailUrl != null)
            .Select(hotel => hotel.ThumbnailUrl)
            .Distinct()
            .ToList();

        if (thumbnailPaths.Count == 0) return mappedHotels;

        var imageUrlsObject = await _imageService.GetImagesUrlsAsync<List<string>>(thumbnailPaths);

        if (imageUrlsObject is List<string> imageUrls)
        {
            MapThumbnailUrls(mappedHotels.Items, imageUrls);
        }

        return mappedHotels;
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
            .And(_hotelExpressions.GetHotelsBasedOnAmenitiesExpression(request.Amenities))
            .And(_hotelExpressions.GetHotelsBasedOnRoomTypeExpression(request.RoomType));
    }

    private void MapThumbnailUrls(List<HotelUserResponse> hotels,
        List<string> imageUrls)
    {
        foreach (var hotel in hotels)
        {
            if (hotel.ThumbnailUrl.IsNullOrEmpty()) continue;
            var matchingUrl = imageUrls.FirstOrDefault(url => url.Contains(hotel.ThumbnailUrl));

            if (matchingUrl != null)
            {
                hotel.ThumbnailUrl = matchingUrl;
            }
        }
    }
}