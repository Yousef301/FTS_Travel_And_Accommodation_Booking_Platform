using AutoMapper;
using MediatR;
using TABP.Application.Queries.SpecialOffers;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.SpecialOffers.CreateSpecialOffer;

public class CreateSpecialOfferCommandHandler : IRequestHandler<CreateSpecialOfferCommand, SpecialOfferResponse>
{
    private readonly ISpecialOfferRepository _specialOfferRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSpecialOfferCommandHandler(ISpecialOfferRepository specialOfferRepository, IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _specialOfferRepository = specialOfferRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SpecialOfferResponse> Handle(CreateSpecialOfferCommand request,
        CancellationToken cancellationToken)
    {
        var specialOffer = _mapper.Map<SpecialOffer>(request);

        specialOffer.Id = Guid.NewGuid();

        var createdOffer = await _specialOfferRepository.CreateAsync(specialOffer);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<SpecialOfferResponse>(createdOffer);
    }
}