using MediatR;
using Microsoft.AspNetCore.Http;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Images.Hotels.CreateHotelThumbnail;

public class CreateHotelThumbnailCommandHandler : IRequestHandler<CreateHotelThumbnailCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly IHotelRepository _hotelRepository;
    private readonly IImageRepository<HotelImage> _hotelImageRepository;

    public CreateHotelThumbnailCommandHandler(IUnitOfWork unitOfWork,
        IImageService imageService,
        IHotelRepository hotelRepository,
        IImageRepository<HotelImage> hotelImageRepository)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _hotelRepository = hotelRepository;
        _hotelImageRepository = hotelImageRepository;
    }

    public async Task Handle(CreateHotelThumbnailCommand request,
        CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.HotelId) ??
                    throw new NotFoundException(nameof(Hotel), request.HotelId);

        hotel.Name = hotel.Name.ToLower();

        var fileExtension = Path.GetExtension(request.Image.FileName);

        await _imageService.UploadImagesAsync(new List<IFormFile> { request.Image },
            new Dictionary<string, object>
            {
                { "folder", "hotels" },
                { "prefix", $"{hotel.Name}" },
                { "fileExtension", $"{fileExtension}" },
            });

        var hotelThumbnail =
            await _hotelImageRepository.GetByIdAsync(hi => hi.HotelId == request.HotelId && hi.Thumbnail);

        if (hotelThumbnail != null)
        {
            hotelThumbnail.ImagePath = $"hotels/{hotel.Name}_{request.Image.FileName}".Replace(' ', '_');

            await _unitOfWork.SaveChangesAsync();
        }
        else
        {
            hotelThumbnail = new HotelImage
            {
                Id = new Guid(),
                HotelId = request.HotelId,
                ImagePath = $"hotels/{hotel.Name}_{request.Image.FileName}".Replace(' ', '_'),
                Thumbnail = true
            };

            await _hotelImageRepository.CreateAsync(hotelThumbnail);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}