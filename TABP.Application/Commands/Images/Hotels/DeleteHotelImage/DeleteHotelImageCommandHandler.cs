using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Images.Hotels.DeleteHotelImage;

public class DeleteHotelImageCommandHandler : IRequestHandler<DeleteHotelImageCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly IImageRepository<HotelImage> _hotelImageRepository;

    public DeleteHotelImageCommandHandler(IUnitOfWork unitOfWork,
        IImageService imageService,
        IImageRepository<HotelImage> hotelImageRepository)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _hotelImageRepository = hotelImageRepository;
    }

    public async Task Handle(DeleteHotelImageCommand request,
        CancellationToken cancellationToken)
    {
        var hotelImagePath = await _hotelImageRepository.GetImagePathAsync(request.ImageId, request.HotelId) ??
                             throw new NotFoundException("Hotel Image", request.ImageId);

        await _imageService.DeleteImageAsync(hotelImagePath);

        await _hotelImageRepository.DeleteAsync(request.ImageId);

        await _unitOfWork.SaveChangesAsync();
    }
}