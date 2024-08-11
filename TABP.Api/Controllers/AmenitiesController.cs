using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.Amenities.CreateAmenity;
using TABP.Application.Commands.Amenities.DeleteAmenity;
using TABP.Application.Commands.Amenities.UpdateAmenity;
using TABP.Application.Queries.Amenities.GetAmenities;
using TABP.Application.Queries.Amenities.GetAmenityById;
using TABP.Domain.Enums;
using TABP.Web.DTOs.Amenities;

namespace TABP.Web.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{v:apiVersion}/amenities")]
[Authorize(Roles = nameof(Role.Admin))]
public class AmenitiesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AmenitiesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAmenities()
    {
        var amenities = await _mediator.Send(new GetAmenitiesQuery());

        return Ok(amenities);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAmenity(Guid id)
    {
        var amenity = await _mediator.Send(new GetAmenityByIdQuery { Id = id });

        return Ok(amenity);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAmenity([FromBody] CreateAmenityDto createAmenityDto)
    {
        var command = _mapper.Map<CreateAmenityCommand>(createAmenityDto);

        var amenity = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetAmenity), new { id = amenity.Id }, amenity);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAmenity(Guid id)
    {
        await _mediator.Send(new DeleteAmenityCommand { Id = id });

        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateAmenity(Guid id,
        [FromBody] JsonPatchDocument<UpdateAmenityDto> amenityUpdateDto)
    {
        var amenityDocument = _mapper.Map<JsonPatchDocument<AmenityUpdate>>(amenityUpdateDto);

        await _mediator.Send(new UpdateAmenityCommand { Id = id, AmenityDocument = amenityDocument });

        return Ok();
    }
}