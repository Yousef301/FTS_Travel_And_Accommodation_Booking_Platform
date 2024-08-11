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

    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto createRoomDto, Guid hotelId)
    {
        var command = _mapper.Map<CreateRoomCommand>(createRoomDto);

        command.HotelId = hotelId;

        await _mediator.Send(command);

        return Created();
    }

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