using MediatR;

namespace TABP.Application.Commands.Reviews.CreateReview;

public class CreateReviewCommand : IRequest
{
    public Guid HotelId { get; set; }
    public Guid UserId { get; set; }
    public string Comment { get; set; }
    public double Rate { get; set; }
}