using AutoMapper;
using MediatR;
using TABP.Application.Queries.Amenities;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Amenities.CreateAmenity;

public class CreateAmenityCommandHandler : IRequestHandler<CreateAmenityCommand>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAmenityCommandHandler(IAmenityRepository amenityRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _amenityRepository = amenityRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(CreateAmenityCommand request, CancellationToken cancellationToken)
    {
        if (await _amenityRepository.ExistsAsync(a => a.Name.ToLower() == request.Name.ToLower()))
            throw new UniqueConstraintViolationException("An amenity with the same name already exists.");

        var amenity = _mapper.Map<Amenity>(request);

        amenity.Id = Guid.NewGuid();

        await _amenityRepository.CreateAsync(amenity);

        await _unitOfWork.SaveChangesAsync();
    }
}