using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.RoomAmenities.CreateRoomAmenity;
using TABP.Application.Commands.RoomAmenities.DeleteRoomAmenity;
using TABP.Application.Queries.RoomAmenities.GetRoomAmenities;
using TABP.Shared.Enums;
using TABP.Web.DTOs.RoomAmenities;

namespace TABP.Web.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{v:apiVersion}/rooms/{roomId:guid}/amenities")]
[Authorize(Roles = nameof(Role.Admin))]
public class RoomAmenitiesController : ControllerBase
{
    private IMapper _mapper;
    private IMediator _mediator;

    public RoomAmenitiesController(IMapper mapper,
        IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves the list of amenities for a specific room.
    /// </summary>
    /// <param name="roomId">The ID of the room for which to retrieve amenities.</param>
    /// <returns>A list of amenities associated with the specified room.</returns>
    /// <response code="200">The amenities were successfully retrieved.</response>
    /// <response code="404">If the room is not found.</response>
    /// <response code="500">If an internal server error occurs while retrieving the amenities.</response>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetRoomAmenities(Guid roomId)
    {
        var amenities = await _mediator.Send(new GetRoomAmenitiesQuery
        {
            RoomId = roomId
        });

        return Ok(amenities);
    }

    /// <summary>
    /// Adds a new amenity to a specific room.
    /// </summary>
    /// <param name="roomId">The ID of the room to which the amenity will be added.</param>
    /// <param name="request">The details of the amenity to be added.</param>
    /// <response code="201">The amenity was successfully added to the room.</response>
    /// <response code="400">If the request contains invalid data or the room does not exist.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to add an amenity to this room.</response>
    /// <response code="500">If an internal server error occurs while adding the amenity.</response>
    [HttpPost]
    public async Task<IActionResult> CreateRoomAmenity(Guid roomId,
        CreateRoomAmenityDto request)
    {
        var command = _mapper.Map<CreateRoomAmenityCommand>(request);

        command.RoomId = roomId;

        await _mediator.Send(command);

        return Created();
    }

    /// <summary>
    /// Deletes an amenity from a specific room.
    /// </summary>
    /// <param name="id">The ID of the amenity to be deleted.</param>
    /// <param name="roomId">The ID of the room from which the amenity will be deleted.</param>
    /// <response code="204">The amenity was successfully deleted from the room.</response>
    /// <response code="400">If the request is invalid or the amenity does not exist.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to delete an amenity from this room.</response>
    /// <response code="404">If the room amenity is not found.</response>
    /// <response code="500">If an internal server error occurs while deleting the amenity.</response>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRoomAmenity(Guid id,
        Guid roomId)
    {
        await _mediator.Send(new DeleteRoomAmenityCommand
        {
            RoomId = roomId,
            Id = id
        });

        return NoContent();
    }
}