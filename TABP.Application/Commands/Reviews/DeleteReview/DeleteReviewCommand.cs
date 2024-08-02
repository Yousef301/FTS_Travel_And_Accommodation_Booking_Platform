using MediatR;

namespace TABP.Application.Commands.Reviews.DeleteReview;

public class DeleteReviewCommand : IRequest
{
    public Guid HotelId { get; set; }
    public Guid UserId { get; set; }
    public Guid ReviewId { get; set; }
}