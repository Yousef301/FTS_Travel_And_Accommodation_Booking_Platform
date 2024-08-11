using AutoMapper;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Hotels.GetHotelsWithFeaturedDeals;

public class
    GetHotelsWithFeaturedDealsQueryHandler : IRequestHandler<GetHotelsWithFeaturedDealsQuery,
    IEnumerable<HotelWithFeaturedDealResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    public GetHotelsWithFeaturedDealsQueryHandler(IHotelRepository hotelRepository, IMapper mapper,
        IImageService imageService)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
        _imageService = imageService;
    }

    public async Task<IEnumerable<HotelWithFeaturedDealResponse>> Handle(GetHotelsWithFeaturedDealsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Count is <= 0 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Count),
                "The count must be between 1 and 5 inclusive.");
        }

        var hotels = await _hotelRepository.GetHotelsWithDealsAsync(request.Count);

        var mappedHotels = _mapper
            .Map<List<HotelWithFeaturedDealResponse>>(hotels);

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

    private void MapThumbnailUrls(List<HotelWithFeaturedDealResponse> hotels, List<string> imageUrls)
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