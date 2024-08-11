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

    /// <summary>
    /// Retrieves reviews for a specific hotel.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel for which to retrieve reviews.</param>
    /// <returns>A list of reviews for the specified hotel.</returns>
    /// <response code="200">Returns the list of reviews for the hotel.</response>
    /// <response code="404">If the hotel with the specified ID is not found.</response>
    /// <response code="500">If an internal server error occurs while retrieving the reviews.</response>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult> GetHotelReviews(Guid hotelId)
    {
        var reviews = await _mediator.Send(new GetHotelReviewsQuery
        {
            HotelId = hotelId
        });

        return Ok(reviews);
    }

    /// <summary>
    /// Retrieves reviews written by the currently authenticated user for a specific hotel.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel for which to retrieve the user's reviews.</param>
    /// <returns>A list of reviews written by the user for the specified hotel.</returns>
    /// <response code="200">Returns the list of the user's reviews for the hotel.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user doesn't have permission.</response>
    /// <response code="404">If the user has not written any reviews for the specified hotel.</response>
    /// <response code="500">If an internal server error occurs while retrieving the reviews.</response>
    [HttpGet("user-reviews")]
    public async Task<ActionResult> GetUserHotelReviews(Guid hotelId)
    {
        var userReviews = await _mediator.Send(new GetUserHotelReviewsQuery
        {
            UserId = _userContext.Id,
            HotelId = hotelId
        });

        return Ok(userReviews);
    }

    /// <summary>
    /// Creates a new review for a specific hotel.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel to which the review applies.</param>
    /// <param name="createReviewDto">The review details provided by the user.</param>
    /// <response code="201">The review was successfully created.</response>
    /// <response code="400">If the request contains invalid data or is missing required fields.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user does not have permission to create a review for the hotel.</response>
    /// <response code="409">If a review for the same hotel by the same user already exists.</response>
    /// <response code="500">If an internal server error occurs while creating the review.</response>
    [HttpPost]
    public async Task<IActionResult> CreateReview(Guid hotelId, [FromBody] CreateReviewDto createReviewDto)
    {
        var command = _mapper.Map<CreateReviewCommand>(createReviewDto);
        command.HotelId = hotelId;
        command.UserId = _userContext.Id;

        await _mediator.Send(command);

        return Created();
    }

    /// <summary>
    /// Deletes a review for a specific hotel.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel to which the review applies.</param>
    /// <param name="reviewId">The ID of the review to delete.</param>
    /// <response code="204">The review was successfully deleted.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user does not have permission to delete the review.</response>
    /// <response code="404">If the review is not found.</response>
    /// <response code="500">If an internal server error occurs while deleting the review.</response>
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

    /// <summary>
    /// Updates a review for a specific hotel using a JSON patch document.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel to which the review applies.</param>
    /// <param name="reviewId">The ID of the review to update.</param>
    /// <param name="reviewUpdateDto">The JSON patch document containing the updates for the review.</param>
    /// <response code="200">The review was successfully updated.</response>
    /// <response code="400">If the request contains invalid data or the JSON patch document is malformed.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user does not have permission to update the review.</response>
    /// <response code="404">If the review is not found.</response>
    /// <response code="500">If an internal server error occurs while updating the review.</response>
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