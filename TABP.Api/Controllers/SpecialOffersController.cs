using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.SpecialOffers.CreateSpecialOffer;
using TABP.Application.Commands.SpecialOffers.DeleteSpecialOffer;
using TABP.Application.Commands.SpecialOffers.UpdateSpecialOffer;
using TABP.Application.Queries.SpecialOffers.GetSpecialOffers;
using TABP.Domain.Enums;
using TABP.Web.DTOs.SpecialOffers;

namespace TABP.Web.Controllers;

[ApiController]
[Route("api/rooms/{roomId}/special-offers")]
[Authorize(Roles = nameof(Role.Admin))]
public class SpecialOffersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SpecialOffersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetSpecialOffers(Guid roomId)
    {
        var specialOffers = await _mediator.Send(
            new GetSpecialOfferQuery() { RoomId = roomId });

        return Ok(specialOffers);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSpecialOffer(Guid roomId, [FromBody] CreateSpecialOfferDto request)
    {
        var command = _mapper.Map<CreateSpecialOfferCommand>(request);
        command.RoomId = roomId;

        var specialOffer = await _mediator.Send(command);

        return Ok(specialOffer);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSpecialOffer(Guid roomId, Guid id)
    {
        await _mediator.Send(new DeleteSpecialOfferCommand { RoomId = roomId, Id = id });

        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateSpecialOffer(Guid roomId, Guid id,
        [FromBody] JsonPatchDocument<UpdateSpecialOfferDto> request)
    {
        var mappedRequest = _mapper.Map<JsonPatchDocument<SpecialOfferUpdate>>(request);

        var command = new UpdateSpecialOfferCommand
        {
            Id = id,
            RoomId = roomId, SpecialOfferDocument = mappedRequest
        };

        var specialOffer = await _mediator.Send(command);

        return Ok(specialOffer);
    }
}