﻿using AutoMapper;
using MediatR;
using TABP.Application.Queries.Hotels;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.Hotels.CreateHotel;

public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateHotelCommandHandler(IHotelRepository hotelRepository, IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = _mapper.Map<Hotel>(request);

        hotel.Id = Guid.NewGuid();

        await _hotelRepository.CreateAsync(hotel);

        await _unitOfWork.SaveChangesAsync();
    }
}