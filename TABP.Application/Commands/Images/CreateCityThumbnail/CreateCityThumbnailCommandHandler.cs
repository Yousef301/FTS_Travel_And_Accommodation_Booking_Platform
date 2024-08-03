using MediatR;
using Microsoft.AspNetCore.Http;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using System.IO; // Add this for Path.GetExtension

namespace TABP.Application.Commands.Images.CreateCityThumbnail;

public class CreateCityThumbnailCommandHandler : IRequestHandler<CreateCityThumbnailCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly ICityRepository _cityRepository;
    private readonly ICityImageRepository _cityImageRepository;

    public CreateCityThumbnailCommandHandler(IUnitOfWork unitOfWork, IImageService imageService,
        ICityRepository cityRepository, ICityImageRepository cityImageRepository)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _cityRepository = cityRepository;
        _cityImageRepository = cityImageRepository;
    }

    public async Task Handle(CreateCityThumbnailCommand request, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(request.CityId);
        city.Name = city.Name.ToLower();

        var fileExtension = Path.GetExtension(request.Image.FileName);
        var imagePath = $"cities/{city.Name}_thumbnail{fileExtension}";

        await _imageService.UploadImagesAsync(new List<IFormFile> { request.Image },
            new Dictionary<string, object>
            {
                { "folder", "cities" },
                { "prefix", $"{city.Name}" },
                { "fileExtension", $"{fileExtension}" },
                { "thumbnail", true }
            });

        var cityThumbnail = new CityImage
        {
            Id = new Guid(),
            CityId = request.CityId,
            ImagePath = imagePath,
            Description = "Thumbnail"
        };

        await _cityImageRepository.CreateAsync(cityThumbnail);
        await _unitOfWork.SaveChangesAsync();
    }
}