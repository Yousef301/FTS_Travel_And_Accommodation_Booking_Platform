using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

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
        await _hotelRepository.DeleteAsync(request.Id);

        await _unitOfWork.SaveChangesAsync();
    }
}