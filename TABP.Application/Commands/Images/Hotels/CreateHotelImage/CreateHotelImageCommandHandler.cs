using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Images.Hotels.CreateHotelImage;

public class CreateHotelImageCommandHandler : IRequestHandler<CreateHotelImageCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly IHotelRepository _hotelRepository;
    private readonly IImageRepository<HotelImage> _hotelImageRepository;

    public CreateHotelImageCommandHandler(IUnitOfWork unitOfWork, IImageService imageService,
        IHotelRepository hotelRepository, IImageRepository<HotelImage> hotelImageRepository)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _hotelRepository = hotelRepository;
        _hotelImageRepository = hotelImageRepository;
    }

    public async Task Handle(CreateHotelImageCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.HotelId) ??
                    throw new NotFoundException($"Hotel with ID {request.HotelId} wasn't found");

        hotel.Name = hotel.Name.ToLower();

        await _imageService.UploadImagesAsync(request.Images, new Dictionary<string, object>
        {
            { "folder", "hotels" },
            { "prefix", hotel.Name },
        });

        var hotelImages = request.Images.Select(image => new HotelImage
        {
            Id = new Guid(),
            HotelId = request.HotelId,
            ImagePath = $"hotels/{hotel.Name}_{image.FileName}".Replace(' ', '_'),
            Thumbnail = false
        });

        await _hotelImageRepository.AddRangeAsync(hotelImages);
        await _unitOfWork.SaveChangesAsync();
    }
}