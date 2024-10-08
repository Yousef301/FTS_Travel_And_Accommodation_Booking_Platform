﻿using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Shared.Exceptions;

namespace TABP.Application.Commands.Cities.DeleteCity;

public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;


    public DeleteCityCommandHandler(ICityRepository cityRepository,
        IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCityCommand request,
        CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(request.Id) ??
                   throw new NotFoundException(nameof(City), request.Id);

        _cityRepository.Delete(city);

        await _unitOfWork.SaveChangesAsync();
    }
}