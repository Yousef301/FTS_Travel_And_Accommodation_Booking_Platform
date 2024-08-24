using AutoMapper;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Hotels.GetRecentlyVisitedHotels;

public class GetRecentlyVisitedHotelsQueryHandler : IRequestHandler<GetRecentlyVisitedHotelsQuery,
    IEnumerable<RecentlyVisitedHotelsResponse>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    public GetRecentlyVisitedHotelsQueryHandler(IBookingRepository bookingRepository,
        IHotelRepository hotelRepository,
        IImageService imageService,
        IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _hotelRepository = hotelRepository;
        _mapper = mapper;
        _imageService = imageService;
    }

    public async Task<IEnumerable<RecentlyVisitedHotelsResponse>> Handle(GetRecentlyVisitedHotelsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Count is <= 0 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Count),
                "The count must be between 1 and 5 inclusive.");
        }

        var hotels = new List<Hotel>();

        var hotelsId = await _bookingRepository
            .GetRecentlyBookedHotelsIdByUserAsync(request.UserId, request.Count);

        foreach (var hotelId in hotelsId)
        {
            var hotel = await _hotelRepository.GetByIdAsync(hotelId, true, true);

            if (hotel is not null) hotels.Add(hotel);
        }


        var mappedHotels = _mapper.Map<List<RecentlyVisitedHotelsResponse>>(hotels);

        var thumbnailPaths = mappedHotels
            .Where(hotel => hotel.ThumbnailUrl != null)
            .Select(hotel => hotel.ThumbnailUrl)
            .Distinct()
            .ToList();

        if (thumbnailPaths.Count == 0) return mappedHotels;

        var imageUrlsObject = await _imageService.GetImagesUrlsAsync<List<string>>(thumbnailPaths);

        if (imageUrlsObject is List<string> imageUrls)
        {
            MapThumbnailUrls(mappedHotels, imageUrls);
        }

        return mappedHotels;
    }

    private void MapThumbnailUrls(List<RecentlyVisitedHotelsResponse> hotels,
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