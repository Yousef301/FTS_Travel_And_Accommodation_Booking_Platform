using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.Cities.DeleteCity;

public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand>
{
    private readonly ICityRepository _amenityRepository;
    private readonly IUnitOfWork _unitOfWork;


    public DeleteCityCommandHandler(ICityRepository amenityRepository, IUnitOfWork unitOfWork)
    {
        _amenityRepository = amenityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        await _amenityRepository.DeleteAsync(request.Id);

        await _unitOfWork.SaveChangesAsync();
    }
}