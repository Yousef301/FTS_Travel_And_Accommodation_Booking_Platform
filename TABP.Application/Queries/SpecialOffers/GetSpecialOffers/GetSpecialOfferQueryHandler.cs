using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.SpecialOffers.GetSpecialOffers;

public class GetSpecialOfferQueryHandler : IRequestHandler<GetSpecialOfferQuery, IEnumerable<SpecialOfferResponse>>
{
    private readonly ISpecialOfferRepository _specialOfferRepository;
    private readonly IMapper _mapper;

    public GetSpecialOfferQueryHandler(ISpecialOfferRepository specialOfferRepository,
        IMapper mapper)
    {
        _specialOfferRepository = specialOfferRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SpecialOfferResponse>> Handle(GetSpecialOfferQuery request,
        CancellationToken cancellationToken)
    {
        var specialOffers = await _specialOfferRepository
            .GetRoomOffersAsync(request.RoomId);

        return _mapper.Map<IEnumerable<SpecialOfferResponse>>(specialOffers);
    }
}