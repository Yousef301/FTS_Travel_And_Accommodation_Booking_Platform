using AutoMapper;
using MediatR;
using TABP.DAL.Entities;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Queries.SpecialOffers.GetSpecialOffers;

public class GetSpecialOfferQueryHandler : IRequestHandler<GetSpecialOfferQuery, IEnumerable<SpecialOfferResponse>>
{
    private readonly ISpecialOfferRepository _specialOfferRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public GetSpecialOfferQueryHandler(ISpecialOfferRepository specialOfferRepository,
        IRoomRepository roomRepository,
        IMapper mapper)
    {
        _specialOfferRepository = specialOfferRepository;
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SpecialOfferResponse>> Handle(GetSpecialOfferQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _roomRepository.ExistsAsync(r => r.Id == request.RoomId))
        {
            throw new NotFoundException(nameof(Room), request.RoomId);
        }

        var specialOffers = await _specialOfferRepository
            .GetRoomOffersAsync(request.RoomId);

        return _mapper.Map<IEnumerable<SpecialOfferResponse>>(specialOffers);
    }
}