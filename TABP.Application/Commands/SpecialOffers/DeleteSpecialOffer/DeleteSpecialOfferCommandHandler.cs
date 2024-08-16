using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.SpecialOffers.DeleteSpecialOffer;

public class DeleteSpecialOfferCommandHandler : IRequestHandler<DeleteSpecialOfferCommand>
{
    private readonly ISpecialOfferRepository _specialOfferRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSpecialOfferCommandHandler(IUnitOfWork unitOfWork,
        ISpecialOfferRepository specialOfferRepository,
        IRoomRepository roomRepository)
    {
        _unitOfWork = unitOfWork;
        _specialOfferRepository = specialOfferRepository;
        _roomRepository = roomRepository;
    }

    public async Task Handle(DeleteSpecialOfferCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _roomRepository.ExistsAsync(r => r.Id == request.RoomId))
            throw new NotFoundException($"Room with id {request.RoomId} wasn't found.");

        var specialOffer = await _specialOfferRepository.GetByIdAsync(request.Id) ??
                           throw new NotFoundException($"Room with id {request.RoomId} wasn't found.");


        _specialOfferRepository.Delete(specialOffer);

        await _unitOfWork.SaveChangesAsync();
    }
}