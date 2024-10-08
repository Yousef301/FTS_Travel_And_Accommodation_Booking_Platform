﻿using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Images.Hotels.GetHotelImages;

public class GetHotelImagesQueryHandler : IRequestHandler<GetHotelImagesQuery, IEnumerable<Dictionary<string, string>>>
{
    private readonly IImageRepository<HotelImage> _hotelImageRepository;
    private readonly IImageService _imageService;

    public GetHotelImagesQueryHandler(IImageRepository<HotelImage> hotelImageRepository,
        IImageService imageService)
    {
        _hotelImageRepository = hotelImageRepository;
        _imageService = imageService;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> Handle(GetHotelImagesQuery request,
        CancellationToken cancellationToken)
    {
        var hotelImages = await _hotelImageRepository.GetImagesPathAsync(request.HotelId);
        
        var imagesObject = await _imageService.GetImagesUrlsAsync<IEnumerable<Dictionary<string, string>>>(hotelImages);

        if (imagesObject is IEnumerable<Dictionary<string, string>> images)
        {
            return images;
        }

        return new List<Dictionary<string, string>>();
    }
}