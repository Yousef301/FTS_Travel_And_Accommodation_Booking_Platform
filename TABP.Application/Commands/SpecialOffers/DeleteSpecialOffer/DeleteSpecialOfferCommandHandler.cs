using MediatR;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.SpecialOffers.DeleteSpecialOffer;

public class DeleteSpecialOfferCommandHandler : IRequestHandler<DeleteSpecialOfferCommand>
{
    private readonly ISpecialOfferRepository _specialOfferRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSpecialOfferCommandHandler(IUnitOfWork unitOfWork, ISpecialOfferRepository specialOfferRepository)
    {
        _unitOfWork = unitOfWork;
        _specialOfferRepository = specialOfferRepository;
    }

    public async Task Handle(DeleteSpecialOfferCommand request, CancellationToken cancellationToken)
    {
        await _specialOfferRepository.DeleteAsync(request.Id, request.RoomId);

        await _unitOfWork.SaveChangesAsync();
    }
}