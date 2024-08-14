using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Amenities.UpdateAmenity;

public class UpdateAmenityCommandHandler : IRequestHandler<UpdateAmenityCommand>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateAmenityCommandHandler(IAmenityRepository amenityRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _amenityRepository = amenityRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateAmenityCommand request, CancellationToken cancellationToken)
    {
        var amenity = await _amenityRepository.GetByIdAsync(request.Id) ??
                      throw new NotFoundException($"Amenity with id {request.Id} not found.");

        var updatedAmenityDto = _mapper.Map<AmenityUpdate>(amenity);

        request.AmenityDocument.ApplyTo(updatedAmenityDto);

        _mapper.Map(updatedAmenityDto, amenity);

        if (await _amenityRepository.ExistsAsync(a => a.Name.ToLower() == amenity.Name.ToLower()))
            throw new UniqueConstraintViolationException("An amenity with the same name already exists.");

        _amenityRepository.Update(amenity);

        await _unitOfWork.SaveChangesAsync();
    }
}