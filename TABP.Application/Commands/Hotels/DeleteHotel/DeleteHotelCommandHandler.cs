using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Hotels.DeleteHotel;

public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteHotelCommandHandler(IUnitOfWork unitOfWork, IHotelRepository hotelRepository)
    {
        _unitOfWork = unitOfWork;
        _hotelRepository = hotelRepository;
    }

    public async Task Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.Id))
        {
            throw new NotFoundException($"Hotel with id {request.Id} wasn't found");
        }

        await _hotelRepository.DeleteAsync(request.Id);

        await _unitOfWork.SaveChangesAsync();
    }
}