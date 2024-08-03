using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.Images.Cities.DeleteCityImage;

public class DeleteCityImageCommandHandler : IRequestHandler<DeleteCityImageCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly IImageRepository<CityImage> _cityImageRepository;

    public DeleteCityImageCommandHandler(IUnitOfWork unitOfWork, IImageService imageService,
        IImageRepository<CityImage> cityImageRepository)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _cityImageRepository = cityImageRepository;
    }

    public async Task Handle(DeleteCityImageCommand request, CancellationToken cancellationToken)
    {
        var cityImagesPath = await _cityImageRepository.GetImagePathAsync(request.ImageId);

        await _imageService.DeleteImageAsync(cityImagesPath);

        await _cityImageRepository.DeleteAsync(request.ImageId);

        await _unitOfWork.SaveChangesAsync();
    }
}