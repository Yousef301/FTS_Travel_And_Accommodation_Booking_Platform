using AutoMapper;
using MediatR;
using TABP.Application.Queries.Amenities;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.Amenities.CreateAmenity;

public class CreateAmenityCommandHandler : IRequestHandler<CreateAmenityCommand, AmenityResponse>
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

    public async Task<AmenityResponse> Handle(CreateAmenityCommand request, CancellationToken cancellationToken)
    {
        var amenity = _mapper.Map<Amenity>(request);

        amenity.Id = Guid.NewGuid();

        var createdAmenity = await _amenityRepository.CreateAsync(amenity);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AmenityResponse>(createdAmenity);
    }
}