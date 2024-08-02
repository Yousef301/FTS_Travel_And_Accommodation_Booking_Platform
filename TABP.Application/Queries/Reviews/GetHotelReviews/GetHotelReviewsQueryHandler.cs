using AutoMapper;
using MediatR;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Reviews.GetHotelReviews;

public class GetHotelReviewsQueryHandler : IRequestHandler<GetHotelReviewsQuery, IEnumerable<ReviewResponse>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public GetHotelReviewsQueryHandler(IMapper mapper, IReviewRepository reviewRepository)
    {
        _mapper = mapper;
        _reviewRepository = reviewRepository;
    }

    public async Task<IEnumerable<ReviewResponse>> Handle(GetHotelReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var reviews = await _reviewRepository
            .GetByHotelIdAsync(request.HotelId);

        return _mapper.Map<IEnumerable<ReviewResponse>>(reviews);
    }
}