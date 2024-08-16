using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Hotels.DeleteHotel;

public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteHotelCommandHandler(IUnitOfWork unitOfWork,
        IHotelRepository hotelRepository)
    {
        _unitOfWork = unitOfWork;
        _hotelRepository = hotelRepository;
    }

    public async Task Handle(DeleteHotelCommand request,
        CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.Id) ??
                    throw new NotFoundException($"Hotel with id {request.Id} wasn't found");

        _hotelRepository.Delete(hotel);

        await _unitOfWork.SaveChangesAsync();
    }
}