﻿using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.Images.DeleteCityImage;

public class DeleteCityImageCommandHandler : IRequestHandler<DeleteCityImageCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly ICityImageRepository _cityImageRepository;

    public DeleteCityImageCommandHandler(IUnitOfWork unitOfWork, IImageService imageService,
        ICityImageRepository cityImageRepository)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _cityImageRepository = cityImageRepository;
    }

    public async Task Handle(DeleteCityImageCommand request, CancellationToken cancellationToken)
    {
        var cityImagesPath = await _cityImageRepository.GetCityImagePathAsync(request.ImageId);

        await _imageService.DeleteImageAsync(cityImagesPath);

        await _cityImageRepository.DeleteAsync(request.ImageId);

        await _unitOfWork.SaveChangesAsync();
    }
}