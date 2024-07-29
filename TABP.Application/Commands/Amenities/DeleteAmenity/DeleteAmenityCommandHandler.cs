using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

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
        await _amenityRepository.DeleteAsync(request.Id);

        await _unitOfWork.SaveChangesAsync();
    }
}