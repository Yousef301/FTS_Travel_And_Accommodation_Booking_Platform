﻿using AutoMapper;
using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Shared.Exceptions;

namespace TABP.Application.Queries.Amenities.GetAmenityById;

public class GetAmenityByIdQueryHandler : IRequestHandler<GetAmenityByIdQuery, AmenityResponse>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IMapper _mapper;

    public GetAmenityByIdQueryHandler(IAmenityRepository amenityRepository,
        IMapper mapper)
    {
        _amenityRepository = amenityRepository;
        _mapper = mapper;
    }

    public async Task<AmenityResponse> Handle(GetAmenityByIdQuery request,
        CancellationToken cancellationToken)
    {
        var amenity = await _amenityRepository.GetByIdAsync(request.Id) ??
                      throw new NotFoundException(nameof(Amenity), request.Id);

        return _mapper.Map<AmenityResponse>(amenity);
    }
}