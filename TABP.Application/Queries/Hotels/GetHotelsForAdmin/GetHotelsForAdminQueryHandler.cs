using AutoMapper;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using TABP.Application.Helpers.Interfaces;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Queries.Hotels.GetHotelsForAdmin;

public class GetHotelsForAdminQueryHandler : IRequestHandler<GetHotelsForAdminQuery, PagedList<HotelAdminResponse>>
{
    private readonly IHotelExpressions _hotelExpressions;
    private readonly IHotelRepository _hotelRepository;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    public GetHotelsForAdminQueryHandler(IHotelRepository hotelRepository,
        IMapper mapper,
        IHotelExpressions hotelExpressions,
        IImageService imageService)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
        _hotelExpressions = hotelExpressions;
        _imageService = imageService;
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

        var mappedHotels = _mapper.Map<PagedList<HotelAdminResponse>>(hotels);

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

    private void MapThumbnailUrls(List<HotelAdminResponse> hotels,
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