using MediatR;
using Microsoft.AspNetCore.Http;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Shared.Exceptions;

namespace TABP.Application.Commands.Images.Cities.CreateCityThumbnail;

public class CreateCityThumbnailCommandHandler : IRequestHandler<CreateCityThumbnailCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly ICityRepository _cityRepository;

    public CreateCityThumbnailCommandHandler(IUnitOfWork unitOfWork,
        IImageService imageService,
        ICityRepository cityRepository)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _cityRepository = cityRepository;
    }

    public async Task Handle(CreateCityThumbnailCommand request,
        CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(request.CityId) ??
                   throw new NotFoundException(nameof(City), request.CityId);

        city.Name = city.Name.ToLower();

        var fileExtension = Path.GetExtension(request.Image.FileName);

        await _imageService.UploadImagesAsync(new List<IFormFile> { request.Image },
            new Dictionary<string, object>
            {
                { "folder", "cities" },
                { "prefix", $"{city.Name}" },
                { "fileExtension", $"{fileExtension}" },
            });


        city.ThumbnailUrl = $"cities/{city.Name}_{request.Image.FileName}".Replace(' ', '_');

        await _unitOfWork.SaveChangesAsync();
    }
}