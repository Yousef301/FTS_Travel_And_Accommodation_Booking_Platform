﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.Rooms.CreateRoom;
using TABP.Application.Commands.Rooms.DeleteRoom;
using TABP.Application.Commands.Rooms.UpdateRoom;
using TABP.Application.Queries.Rooms.GetRooms;
using TABP.Web.DTOs.Rooms;
using TABP.Web.Enums;

namespace TABP.Web.Controllers;

[Route("api/hotels/{hotelId:guid}/rooms")]
[Authorize(Roles = nameof(Role.Admin))]
public class RoomController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public RoomController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetRooms(Guid hotelId)
    {
        var rooms = await _mediator.Send(new GetRoomsQuery { HotelId = hotelId });

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
    public async Task<IActionResult> DeleteRoom(Guid roomId)
    {
        await _mediator.Send(new DeleteRoomCommand { Id = roomId });

        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateRoom(Guid id,
        [FromBody] JsonPatchDocument<UpdateRoomDto> roomUpdateDto)
    {
        var roomDocument = _mapper.Map<JsonPatchDocument<RoomUpdate>>(roomUpdateDto);

        await _mediator.Send(new UpdateRoomCommand() { Id = id, RoomDocument = roomDocument });

        return Ok();
    }
}