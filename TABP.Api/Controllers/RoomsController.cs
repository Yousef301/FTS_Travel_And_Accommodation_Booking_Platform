using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.Rooms.CreateRoom;
using TABP.Application.Commands.Rooms.DeleteRoom;
using TABP.Application.Commands.Rooms.UpdateRoom;
using TABP.Application.Queries.Rooms.GetAvailableRooms;
using TABP.Application.Queries.Rooms.GetRoomsForAdmin;
using TABP.Domain.Enums;
using TABP.Domain.Extensions;
using TABP.Web.DTOs;
using TABP.Web.DTOs.Rooms;
using TABP.Web.Extensions;

namespace TABP.Web.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{v:apiVersion}/hotels/{hotelId:guid}/rooms")]
[Authorize(Roles = nameof(Role.Admin))]
public class RoomsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public RoomsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a list of rooms for a specific hotel, with optional filters and pagination for admin use.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel for which to retrieve the rooms.</param>
    /// <param name="filterParameters">The filter parameters to apply to the room retrieval.</param>
    /// <returns>A list of rooms with pagination metadata.</returns>
    /// <response code="200">The list of rooms was successfully retrieved.</response>
    /// <response code="400">If the request contains invalid filter parameters or hotel ID.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to access the rooms for this hotel.</response>
    /// <response code="500">If an internal server error occurs while retrieving the rooms.</response>
    [HttpGet]
    public async Task<IActionResult> GetRoomsForAdmin(Guid hotelId,
        [FromQuery] FilterParameters filterParameters)
    {
        var query = _mapper.Map<GetRoomsForAdminQuery>(filterParameters);
        query.HotelId = hotelId;

        var rooms = await _mediator.Send(query);

        var metadata = rooms.ToMetadata();

        Response.AddPaginationMetadata(metadata, Request);

        return Ok(rooms.Items);
    }

    /// <summary>
    /// Retrieves a list of available rooms for a specific hotel based on the provided search criteria.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel for which to retrieve available rooms.</param>
    /// <param name="getAvailableRoomsDto">The criteria to filter the available rooms.</param>
    /// <returns>A list of available rooms matching the search criteria.</returns>
    /// <response code="200">The list of available rooms was successfully retrieved.</response>
    /// <response code="400">If the request contains invalid search criteria or hotel ID.</response>
    /// <response code="500">If an internal server error occurs while retrieving the available rooms.</response>
    [HttpGet("available")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAvailableRooms(Guid hotelId,
        [FromQuery] GetAvailableRoomsDto getAvailableRoomsDto)
    {
        var query = _mapper.Map<GetAvailableRoomsQuery>(getAvailableRoomsDto);
        query.HotelId = hotelId;

        var rooms = await _mediator.Send(query);

        return Ok(rooms);
    }


    /// <summary>
    /// Creates a new room for a specific hotel.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel where the room will be created.</param>
    /// <param name="createRoomDto">The details required to create a new room.</param>
    /// <response code="201">The room was successfully created.</response>
    /// <response code="400">If the request contains invalid data or is missing required fields.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to create a room for this hotel.</response>
    /// <response code="404">If the hotel is not found.</response>
    /// <response code="500">If an internal server error occurs while creating the room.</response>
    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto createRoomDto, Guid hotelId)
    {
        var command = _mapper.Map<CreateRoomCommand>(createRoomDto);

        command.HotelId = hotelId;

        await _mediator.Send(command);

        return Created();
    }

    /// <summary>
    /// Deletes a room from a specific hotel.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel from which the room will be deleted.</param>
    /// <param name="roomId">The ID of the room to be deleted.</param>
    /// <response code="204">The room was successfully deleted.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to delete the room from this hotel.</response>
    /// <response code="404">If the room or hotel does not exist.</response>
    /// <response code="500">If an internal server error occurs while deleting the room.</response>
    [HttpDelete("{roomId:guid}")]
    public async Task<IActionResult> DeleteRoom(Guid roomId, Guid hotelId)
    {
        await _mediator.Send(new DeleteRoomCommand
        {
            HotelId = hotelId,
            Id = roomId
        });

        return NoContent();
    }

    /// <summary>
    /// Updates the details of a room in a specific hotel.
    /// </summary>
    /// <param name="id">The ID of the room to be updated.</param>
    /// <param name="hotelId">The ID of the hotel that contains the room.</param>
    /// <param name="roomUpdateDto">The JSON Patch document containing the updates to be applied to the room.</param>
     /// <response code="200">The room was successfully updated.</response>
    /// <response code="400">If the request contains invalid data or the JSON Patch document is malformed.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to update the room in this hotel.</response>
    /// <response code="404">If the room or hotel does not exist.</response>
    /// <response code="500">If an internal server error occurs while updating the room.</response>
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateRoom(Guid id, Guid hotelId,
        [FromBody] JsonPatchDocument<UpdateRoomDto> roomUpdateDto)
    {
        var roomDocument = _mapper.Map<JsonPatchDocument<RoomUpdate>>(roomUpdateDto);

        await _mediator.Send(new UpdateRoomCommand()
        {
            HotelId = hotelId,
            Id = id,
            RoomDocument = roomDocument
        });

        return Ok();
    }
}