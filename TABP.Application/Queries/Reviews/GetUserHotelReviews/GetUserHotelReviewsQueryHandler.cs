using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Queries.Reviews.GetUserHotelReviews;

public class GetUserHotelReviewsQueryHandler : IRequestHandler<GetUserHotelReviewsQuery,
    IEnumerable<ReviewResponse>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetUserHotelReviewsQueryHandler(IReviewRepository reviewRepository, IMapper mapper,
        IHotelRepository hotelRepository)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
        _hotelRepository = hotelRepository;
    }

    public async Task<IEnumerable<ReviewResponse>> Handle(GetUserHotelReviewsQuery request,
        CancellationToken cancellationToken)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId))
        {
            throw new NotFoundException($"Hotel with ID {request.HotelId} not found.");
        }

        var reviews = await _reviewRepository
            .GetUserHotelsReviewsAsync(request.HotelId, request.UserId);

        return _mapper.Map<IEnumerable<ReviewResponse>>(reviews);
    }
}