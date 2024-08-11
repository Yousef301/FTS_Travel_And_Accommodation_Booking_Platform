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

[ApiVersion(1.0, Deprecated = true)]
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

    /// <summary>
    /// Retrieves a list of all amenities.
    /// </summary>
    /// <returns>A list containing all available amenities.</returns>
    /// <response code="200">Returns the list of amenities.</response>
    [HttpGet]
    public async Task<IActionResult> GetAmenities()
    {
        var amenities = await _mediator.Send(new GetAmenitiesQuery());

        return Ok(amenities);
    }


    /// <summary>
    /// Retrieves an amenity by its id.
    /// </summary>
    /// <param name="id">The unique identifier of the amenity.</param>
    /// <returns>The requested amenity.</returns>
    /// <response code="200">Returns the requested amenity.</response>
    /// <response code="404">If the requested amenity wasn't found.</response>
    /// <response code="500">If an internal server error occurs while retrieving the amenity.</response>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAmenity(Guid id)
    {
        var amenity = await _mediator.Send(new GetAmenityByIdQuery { Id = id });

        return Ok(amenity);
    }

    /// <summary>
    /// Creates a new amenity.
    /// </summary>
    /// <param name="createAmenityDto">The data required to create a new amenity, including fields such as name, description, etc.</param>
    /// <response code="201">The amenity was created successfully.</response>
    /// <response code="400">The request is invalid due to bad input data.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to create amenities (e.g., not an admin).</response>
    /// <response code="409">An amenity with the same name already exists.</response>
    /// <response code="500">If an internal server error occurs while creating the amenity.</response>
    [HttpPost]
    public async Task<IActionResult> CreateAmenity([FromBody] CreateAmenityDto createAmenityDto)
    {
        var command = _mapper.Map<CreateAmenityCommand>(createAmenityDto);

        await _mediator.Send(command);

        return Created();
    }


    /// <summary>
    /// Deletes an amenity by its id.
    /// </summary>
    /// <param name="id">The unique identifier of the amenity to be deleted.</param>
    /// <response code="204">The amenity was successfully deleted.</response>
    /// <response code="404">The amenity with the specified id was not found.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to delete amenities.</response>
    /// <response code="500">If an internal server error occurs while deleting the amenity.</response>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAmenity(Guid id)
    {
        await _mediator.Send(new DeleteAmenityCommand { Id = id });

        return NoContent();
    }

    /// <summary>
    /// Updates an existing amenity using a JSON Patch document.
    /// </summary>
    /// <param name="id">The unique identifier of the amenity to be updated.</param>
    /// <param name="amenityUpdateDto">The JSON Patch document containing the updates to be applied to the amenity.</param>
    /// <response code="200">The amenity was successfully updated.</response>
    /// <response code="400">The JSON Patch document is invalid or contains errors.</response>
    /// <response code="404">The amenity with the specified id was not found.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to update amenities.</response>
    /// <response code="500">If an internal server error occurs while updating the amenity.</response>
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateAmenity(Guid id,
        [FromBody] JsonPatchDocument<UpdateAmenityDto> amenityUpdateDto)
    {
        var amenityDocument = _mapper.Map<JsonPatchDocument<AmenityUpdate>>(amenityUpdateDto);

        await _mediator.Send(new UpdateAmenityCommand { Id = id, AmenityDocument = amenityDocument });

        return Ok();
    }
}