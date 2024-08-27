using AutoMapper;
using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Shared.Exceptions;

namespace TABP.Application.Commands.Amenities.CreateAmenity;

public class CreateAmenityCommandHandler : IRequestHandler<CreateAmenityCommand, Guid>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAmenityCommandHandler(IAmenityRepository amenityRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _amenityRepository = amenityRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateAmenityCommand request,
        CancellationToken cancellationToken)
    {
        if (await _amenityRepository.ExistsAsync(a => a.Name.ToLower() == request.Name.ToLower()))
            throw new UniqueConstraintViolationException("An amenity with the same name already exists.");

        var amenity = _mapper.Map<Amenity>(request);

        var createdAmenity = await _amenityRepository.CreateAsync(amenity);

        await _unitOfWork.SaveChangesAsync();

        return createdAmenity.Id;
    }
}