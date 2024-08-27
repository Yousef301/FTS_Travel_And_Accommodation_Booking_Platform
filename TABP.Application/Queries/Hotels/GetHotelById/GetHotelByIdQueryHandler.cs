using AutoMapper;
using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Shared.Exceptions;

namespace TABP.Application.Queries.Hotels.GetHotelById;

public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, HotelResponseBase>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IImageService _imageService;
    private readonly IMapper _mapper;

    public GetHotelByIdQueryHandler(IHotelRepository hotelRepository,
        IMapper mapper,
        IImageService imageService)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
        _imageService = imageService;
    }

    public async Task<HotelResponseBase> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.Id)
                    ?? throw new NotFoundException(nameof(Hotel), request.Id);

        var mappedHotel = _mapper.Map<HotelResponseBase>(hotel);

        if (hotel.Images.Count != 0)
        {
            mappedHotel.ThumbnailUrl = await _imageService.GetImageUrlAsync(hotel.Images.FirstOrDefault()!.ImagePath);
        }

        return mappedHotel;
    }
}