using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.RoomAmenities.CreateRoomAmenity;
using TABP.Application.Commands.RoomAmenities.DeleteRoomAmenity;
using TABP.Application.Queries.RoomAmenities.GetRoomAmenities;
using TABP.Domain.Enums;
using TABP.Web.DTOs.RoomAmenities;

namespace TABP.Web.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{v:apiVersion}/hotels/{hotelId:guid}/rooms/{roomId:guid}/amenities")]
[Authorize(Roles = nameof(Role.Admin))]
public class RoomAmenitiesController : ControllerBase
{
    private IMapper _mapper;
    private IMediator _mediator;

    public RoomAmenitiesController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

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

    [HttpPost]
    public async Task<IActionResult> CreateRoomAmenity(Guid roomId, CreateRoomAmenityDto request)
    {
        var command = _mapper.Map<CreateRoomAmenityCommand>(request);

        command.RoomId = roomId;

        await _mediator.Send(command);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRoomAmenity(Guid id, Guid roomId)
    {
        await _mediator.Send(new DeleteRoomAmenityCommand
        {
            RoomId = roomId,
            Id = id
        });

        return Ok();
    }
}