﻿using MediatR;
using TABP.Application.Commands.Images.Hotels.DeleteHotelImage;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.Images.Rooms.DeleteRoomImage;

public class DeleteRoomImageCommandHandler : IRequestHandler<DeleteHotelImageCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly IImageRepository<RoomImage> _roomImageRepository;

    public DeleteRoomImageCommandHandler(IUnitOfWork unitOfWork, IImageService imageService,
        IImageRepository<RoomImage> roomImageRepository)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _roomImageRepository = roomImageRepository;
    }

    public async Task Handle(DeleteHotelImageCommand request, CancellationToken cancellationToken)
    {
        var roomImagePath = await _roomImageRepository.GetImagePathAsync(request.ImageId);

        await _imageService.DeleteImageAsync(roomImagePath);

        await _roomImageRepository.DeleteAsync(request.ImageId);

        await _unitOfWork.SaveChangesAsync();
    }
}