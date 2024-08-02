using MediatR;

namespace TABP.Application.Queries.Reviews.GetHotelReviews;

public class GetHotelReviewsQuery : IRequest<IEnumerable<ReviewResponse>>
{
    public Guid HotelId { get; set; }
}