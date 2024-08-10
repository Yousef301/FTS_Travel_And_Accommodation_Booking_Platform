using MediatR;
using Microsoft.AspNetCore.Http;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Images.Cities.CreateCityThumbnail;

public class CreateCityThumbnailCommandHandler : IRequestHandler<CreateCityThumbnailCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly ICityRepository _cityRepository;
    private readonly IImageRepository<CityImage> _cityImageRepository;

    public CreateCityThumbnailCommandHandler(IUnitOfWork unitOfWork, IImageService imageService,
        ICityRepository cityRepository, IImageRepository<CityImage> cityImageRepository)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _cityRepository = cityRepository;
        _cityImageRepository = cityImageRepository;
    }

    // TODO: Handle if there is already a thumbnail for the city
    public async Task Handle(CreateCityThumbnailCommand request, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(request.CityId) ??
                   throw new NotFoundException($"City with id {request.CityId} wasn't found.");

        city.Name = city.Name.ToLower();

        var fileExtension = Path.GetExtension(request.Image.FileName);

        await _imageService.UploadImagesAsync(new List<IFormFile> { request.Image },
            new Dictionary<string, object>
            {
                { "folder", "cities" },
                { "prefix", $"{city.Name}" },
                { "fileExtension", $"{fileExtension}" },
            });

        var cityThumbnail = new CityImage
        {
            Id = new Guid(),
            CityId = request.CityId,
            ImagePath = $"cities/{city.Name}_{request.Image.FileName}".Replace(' ', '_'),
            Thumbnail = true
        };

        await _cityImageRepository.CreateAsync(cityThumbnail);
        await _unitOfWork.SaveChangesAsync();
    }
}