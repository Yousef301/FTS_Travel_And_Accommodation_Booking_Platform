using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.Reviews.CreateReview;
using TABP.Application.Commands.Reviews.DeleteReview;
using TABP.Application.Commands.Reviews.UpdateReview;
using TABP.Application.Queries.Reviews.GetHotelReviews;
using TABP.Application.Queries.Reviews.GetUserHotelReviews;
using TABP.Domain.Enums;
using TABP.Web.DTOs.Reviews;
using TABP.Web.Services.Interfaces;

namespace TABP.Web.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{v:apiVersion}/hotels/{hotelId}/reviews")]
[Authorize(Roles = nameof(Role.Customer))]
public class ReviewsController : ControllerBase
{
    private readonly IUserContext _userContext;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ReviewsController(IUserContext userContext, IMapper mapper,
        IMediator mediator)
    {
        _userContext = userContext;
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult> GetHotelReviews(Guid hotelId, [FromQuery] string user = null)
    {
        if (!string.IsNullOrEmpty(user) && user.Equals("me", StringComparison.OrdinalIgnoreCase))
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userReviews = await _mediator.Send(new GetUserHotelReviewsQuery
            {
                UserId = _userContext.Id,
                HotelId = hotelId
            });

            return Ok(userReviews);
        }

        var reviews = await _mediator.Send(new GetHotelReviewsQuery
        {
            HotelId = hotelId
        });

        return Ok(reviews);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReview(Guid hotelId, [FromBody] CreateReviewDto createReviewDto)
    {
        var command = _mapper.Map<CreateReviewCommand>(createReviewDto);
        command.HotelId = hotelId;
        command.UserId = _userContext.Id;

        await _mediator.Send(command);

        return Created();
    }

    [HttpDelete("{reviewId:guid}")]
    public async Task<IActionResult> DeleteReview(Guid hotelId, Guid reviewId)
    {
        var command = new DeleteReviewCommand
        {
            HotelId = hotelId,
            ReviewId = reviewId,
            UserId = _userContext.Id
        };

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpPatch("{reviewId:guid}")]
    public async Task<IActionResult> UpdateAmenity(Guid hotelId, Guid reviewId,
        [FromBody] JsonPatchDocument<UpdateReviewDto> reviewUpdateDto)
    {
        var reviewDocument = _mapper.Map<JsonPatchDocument<ReviewUpdate>>(reviewUpdateDto);

        await _mediator.Send(new UpdateReviewCommand
        {
            HotelId = hotelId,
            ReviewId = reviewId,
            UserId = _userContext.Id,
            ReviewDocument = reviewDocument
        });

        return Ok();
    }
}