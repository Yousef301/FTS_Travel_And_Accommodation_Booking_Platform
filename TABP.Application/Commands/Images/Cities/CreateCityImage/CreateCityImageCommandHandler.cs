﻿using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Shared.Exceptions;

namespace TABP.Application.Commands.Images.Cities.CreateCityImage;

public class CreateCityImageCommandHandler : IRequestHandler<CreateCityImageCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly ICityRepository _cityRepository;
    private readonly IImageRepository<CityImage> _cityImageRepository;

    public CreateCityImageCommandHandler(IImageService imageService,
        IImageRepository<CityImage> cityImageRepository,
        IUnitOfWork unitOfWork,
        ICityRepository cityRepository)
    {
        _unitOfWork = unitOfWork;
        _cityRepository = cityRepository;
        _imageService = imageService;
        _cityImageRepository = cityImageRepository;
    }

    public async Task Handle(CreateCityImageCommand request,
        CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(request.CityId) ??
                   throw new NotFoundException(nameof(City), request.CityId);

        city.Name = city.Name.ToLower();

        await _imageService.UploadImagesAsync(request.Images, new Dictionary<string, object>
        {
            { "folder", "cities" },
            { "prefix", city.Name },
        });

        var cityImages = request.Images.Select(image => new CityImage
        {
            CityId = request.CityId,
            ImagePath = $"cities/{city.Name}_{image.FileName}".Replace(' ', '_')
        });

        await _cityImageRepository.AddRangeAsync(cityImages);
        await _unitOfWork.SaveChangesAsync();
    }
}