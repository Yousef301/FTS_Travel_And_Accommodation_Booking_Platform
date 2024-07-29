using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

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
        var amenity = await _amenityRepository.GetByIdAsync(request.Id);

        if (amenity == null)
        {
            throw new Exception("Amenity not found"); // TODO: Create a custom exception
        }

        var updatedAmenityDto = _mapper.Map<AmenityUpdate>(amenity);

        request.amenityDocument.ApplyTo(updatedAmenityDto);

        _mapper.Map(updatedAmenityDto, amenity);

        await _amenityRepository.UpdateAsync(amenity);

        await _unitOfWork.SaveChangesAsync();
    }
}