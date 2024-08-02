using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace TABP.Application.Commands.Reviews.UpdateReview;

public class UpdateReviewCommand : IRequest
{
    public Guid HotelId { get; set; }
    public Guid UserId { get; set; }
    public Guid ReviewId { get; set; }
    public JsonPatchDocument<ReviewUpdate> ReviewDocument { get; set; }
}