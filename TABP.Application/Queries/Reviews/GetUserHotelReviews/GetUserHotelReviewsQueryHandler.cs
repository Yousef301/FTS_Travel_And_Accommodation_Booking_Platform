using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Reviews.GetUserHotelReviews;

public class GetUserHotelReviewsQueryHandler : IRequestHandler<GetUserHotelReviewsQuery,
    IEnumerable<ReviewResponse>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public GetUserHotelReviewsQueryHandler(IReviewRepository reviewRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReviewResponse>> Handle(GetUserHotelReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var reviews = await _reviewRepository
            .GetHotelReviewsForUserAsync(request.HotelId, request.UserId);

        return _mapper.Map<IEnumerable<ReviewResponse>>(reviews);
    }
}