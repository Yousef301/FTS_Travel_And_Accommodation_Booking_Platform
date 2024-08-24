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

    public CreateHotelThumbnailCommandHandler(IUnitOfWork unitOfWork,
        IImageService imageService,
        IHotelRepository hotelRepository)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _hotelRepository = hotelRepository;
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

        hotel.ThumbnailUrl = $"hotels/{hotel.Name}_{request.Image.FileName}".Replace(' ', '_');

        await _unitOfWork.SaveChangesAsync();
    }
}