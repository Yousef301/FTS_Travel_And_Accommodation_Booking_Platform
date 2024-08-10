using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Amenities.DeleteAmenity;

public class DeleteAmenityCommandHandler : IRequestHandler<DeleteAmenityCommand>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAmenityCommandHandler(IAmenityRepository amenityRepository, IUnitOfWork unitOfWork)
    {
        _amenityRepository = amenityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteAmenityCommand request, CancellationToken cancellationToken)
    {
        if (!await _amenityRepository.ExistsAsync(a => a.Id == request.Id))
        {
            throw new NotFoundException($"Amenity with id {request.Id} not found");
        }

        await _amenityRepository.DeleteAsync(request.Id);

        await _unitOfWork.SaveChangesAsync();
    }
}