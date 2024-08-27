using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.SpecialOffers.CreateSpecialOffer;
using TABP.Application.Commands.SpecialOffers.DeleteSpecialOffer;
using TABP.Application.Commands.SpecialOffers.UpdateSpecialOffer;
using TABP.Application.Queries.SpecialOffers.GetSpecialOffers;
using TABP.Shared.Enums;
using TABP.Web.DTOs.SpecialOffers;

namespace TABP.Web.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{v:apiVersion}/rooms/{roomId}/offers")]
[Authorize(Roles = nameof(Role.Admin))]
public class SpecialOffersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SpecialOffersController(IMediator mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves special offers for a specific room.
    /// </summary>
    /// <param name="roomId">The ID of the room for which to retrieve special offers.</param>
    /// <returns>A list of special offers for the specified room.</returns>
    /// <response code="200">The special offers were successfully retrieved.</response>
    /// <response code="500">If an internal server error occurs while retrieving the special offers.</response>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetSpecialOffers(Guid roomId)
    {
        var specialOffers = await _mediator.Send(
            new GetSpecialOfferQuery
            {
                RoomId = roomId
            });

        return Ok(specialOffers);
    }

    /// <summary>
    /// Creates a new special offer for a specific room.
    /// </summary>
    /// <param name="roomId">The ID of the room for which to create the special offer.</param>
    /// <param name="request">The details of the special offer to be created.</param>
    /// <response code="201">The special offer was successfully created.</response>
    /// <response code="400">If the request contains invalid data or is missing required fields.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to create a special offer.</response>
    /// <response code="404">If the room does not exist.</response>
    /// <response code="409">If a conflict occurs, such as an existing active special offer for the same room.</response>
    /// <response code="500">If an internal server error occurs while creating the special offer.</response>
    [HttpPost]
    public async Task<IActionResult> CreateSpecialOffer(Guid roomId,
        [FromBody] CreateSpecialOfferDto request)
    {
        var command = _mapper.Map<CreateSpecialOfferCommand>(request);
        command.RoomId = roomId;

        await _mediator.Send(command);

        return Created();
    }

    /// <summary>
    /// Deletes a special offer for a specific room.
    /// </summary>
    /// <param name="roomId">The ID of the room from which the special offer will be deleted.</param>
    /// <param name="id">The ID of the special offer to be deleted.</param>
    /// <response code="204">The special offer was successfully deleted.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to delete the special offer.</response>
    /// <response code="404">The special offer or the room was not found.</response>
    /// <response code="500">If an internal server error occurs while deleting the special offer.</response>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSpecialOffer(Guid roomId,
        Guid id)
    {
        await _mediator.Send(new DeleteSpecialOfferCommand { RoomId = roomId, Id = id });

        return NoContent();
    }

    /// <summary>
    /// Updates a special offer for a specific room.
    /// </summary>
    /// <param name="roomId">The ID of the room where the special offer is located.</param>
    /// <param name="id">The ID of the special offer to be updated.</param>
    /// <param name="request">The JSON Patch document containing the updates to be applied to the special offer.</param>
    /// <response code="200">The special offer was successfully updated.</response>
    /// <response code="400">If the request contains invalid data or is missing required fields.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to update the special offer.</response>
    /// <response code="404">The special offer or the room was not found.</response>
    /// <response code="500">If an internal server error occurs while updating the special offer.</response>
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateSpecialOffer(Guid roomId,
        Guid id,
        [FromBody] JsonPatchDocument<UpdateSpecialOfferDto> request)
    {
        var mappedRequest = _mapper.Map<JsonPatchDocument<SpecialOfferUpdate>>(request);

        var command = new UpdateSpecialOfferCommand
        {
            Id = id,
            RoomId = roomId, SpecialOfferDocument = mappedRequest
        };

        await _mediator.Send(command);

        return Ok();
    }
}