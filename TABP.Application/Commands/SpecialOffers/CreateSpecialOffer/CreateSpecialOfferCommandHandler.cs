using AutoMapper;
using MediatR;
using TABP.Application.Queries.SpecialOffers;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.SpecialOffers.CreateSpecialOffer;

public class CreateSpecialOfferCommandHandler : IRequestHandler<CreateSpecialOfferCommand, SpecialOfferResponse>
{
    private readonly ISpecialOfferRepository _specialOfferRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSpecialOfferCommandHandler(ISpecialOfferRepository specialOfferRepository, IUnitOfWork unitOfWork,
        IMapper mapper, IRoomRepository roomRepository)
    {
        _specialOfferRepository = specialOfferRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _roomRepository = roomRepository;
    }

    public async Task<SpecialOfferResponse> Handle(CreateSpecialOfferCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _roomRepository.ExistsAsync(r => r.Id == request.RoomId))
            throw new NotFoundException($"Room with id {request.RoomId} wasn't found.");

        if (await _specialOfferRepository.ExistsAsync(so => so.RoomId == request.RoomId && so.IsActive))
            throw new BadRequestException("Special offer for this room already exists.");

        var specialOffer = _mapper.Map<SpecialOffer>(request);

        specialOffer.Id = Guid.NewGuid();

        var createdOffer = await _specialOfferRepository.CreateAsync(specialOffer);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<SpecialOfferResponse>(createdOffer);
    }
}