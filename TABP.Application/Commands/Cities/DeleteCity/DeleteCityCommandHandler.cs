using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Cities.DeleteCity;

public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;


    public DeleteCityCommandHandler(ICityRepository cityRepository, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        if (!await _cityRepository.ExistsAsync(c => c.Id == request.Id))
            throw new NotFoundException($"City with id {request.Id} wasn't found");

        await _cityRepository.DeleteAsync(request.Id);

        await _unitOfWork.SaveChangesAsync();
    }
}