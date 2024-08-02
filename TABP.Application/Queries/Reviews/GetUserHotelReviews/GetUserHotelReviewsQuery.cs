using MediatR;

namespace TABP.Application.Queries.Reviews.GetUserHotelReviews;

public class GetUserHotelReviewsQuery : IRequest<IEnumerable<ReviewResponse>>
{
    public Guid HotelId { get; set; }
    public Guid UserId { get; set; }
}