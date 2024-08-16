using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.Hotels.CreateHotel;
using TABP.Application.Commands.Hotels.DeleteHotel;
using TABP.Application.Commands.Hotels.UpdateHotel;
using TABP.Application.Queries.Hotels.GetHotelsForAdmin;
using TABP.Application.Queries.Hotels.GetHotelsForUser;
using TABP.Application.Queries.Hotels.GetHotelsWithFeaturedDeals;
using TABP.Domain.Enums;
using TABP.Domain.Extensions;
using TABP.Web.DTOs;
using TABP.Web.DTOs.Hotels;
using TABP.Web.Extensions;

namespace TABP.Web.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{v:apiVersion}/hotels")]
[Authorize(Roles = nameof(Role.Admin))]
public class HotelsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public HotelsController(IMediator mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Searches for hotels based on user-specified criteria.
    /// </summary>
    /// <param name="searchDto">The search criteria for filtering hotels.</param>
    /// <returns>A paginated list of hotels that match the search and filter criteria.</returns>
    /// <response code="200">Returns a paginated list of hotels that match the search and filter criteria.</response>
    /// <response code="400">If the search criteria is invalid or if required parameters are missing.</response>
    /// <response code="500">If an internal server error occurs while searching for hotels.</response>
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> GetHotelsForUsers([FromQuery] HotelsSearchDto searchDto)
    {
        var query = _mapper.Map<GetHotelsForUserQuery>(searchDto);

        var hotels = await _mediator.Send(query);

        var metadata = hotels.ToMetadata();

        Response.AddPaginationMetadata(metadata, Request);

        return Ok(hotels.Items);
    }

    /// <summary>
    /// Retrieves a list of hotels that has featured deals.
    /// </summary>
    /// <param name="count">The number of featured deals to retrieve. Defaults to 5 if not specified.</param>
    /// <returns>A list of hotels with featured deals.</returns>
    /// <response code="200">Returns a list of hotels with featured deals.</response>
    /// <response code="400">If the count parameter is less than 1 or greater than 5.</response>
    /// <response code="500">If an internal server error occurs while retrieving featured deals.</response>
    [HttpGet("featured-deals")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFeaturedDeals([FromQuery] int count = 5)
    {
        var hotels = await _mediator.Send(
            new GetHotelsWithFeaturedDealsQuery
            {
                Count = count
            });

        return Ok(hotels);
    }

    /// <summary>
    /// Retrieves a list of hotels for administrative purposes based on filter parameters.
    /// </summary>
    /// <param name="filterParameters">The filter parameters to apply when retrieving hotels.</param>
    /// <returns>A paginated list of hotels that match the filter criteria.</returns>
    /// <response code="200">Returns a paginated list of hotels that match the filter criteria.</response>
    /// <response code="400">If the filter parameters are invalid or missing required fields.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to access this resource.</response>
    /// <response code="500">If an internal server error occurs while retrieving the hotels.</response>
    [HttpGet]
    public async Task<IActionResult> GetHotelsForAdmin([FromQuery] FilterParameters filterParameters)
    {
        var query = _mapper.Map<GetHotelsForAdminQuery>(filterParameters);

        var hotels = await _mediator.Send(query);

        var metadata = hotels.ToMetadata();

        Response.AddPaginationMetadata(metadata, Request);

        return Ok(hotels.Items);
    }


    /// <summary>
    /// Creates a new hotel.
    /// </summary>
    /// <param name="request">The data required to create a new hotel.</param>
    /// <response code="200">Returns the created hotel.</response>
    /// <response code="400">If the request contains invalid data or is missing required fields.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to create a hotel.</response>
    /// <response code="500">If an internal server error occurs while creating the hotel.</response>
    [HttpPost]
    public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDto request)
    {
        var command = _mapper.Map<CreateHotelCommand>(request);

        await _mediator.Send(command);

        return Created();
    }

    /// <summary>
    /// Deletes a hotel by its ID.
    /// </summary>
    /// <param name="id">The ID of the hotel to be deleted.</param>
    /// <response code="204">The hotel was successfully deleted.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to delete a hotel.</response>
    /// <response code="404">If the hotel with the specified ID was not found.</response>
    /// <response code="500">If an internal server error occurs while deleting the hotel.</response>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteHotel(Guid id)
    {
        await _mediator.Send(new DeleteHotelCommand { Id = id });

        return NoContent();
    }

    /// <summary>
    /// Updates a hotel by its ID using a JSON Patch document.
    /// </summary>
    /// <param name="id">The ID of the hotel to be updated.</param>
    /// <param name="hotelUpdateDto">The JSON Patch document containing the updates.</param>
    /// <response code="200">The hotel was successfully updated.</response>
    /// <response code="400">If the JSON Patch document is invalid or contains invalid operations.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to update a hotel.</response>
    /// <response code="404">If the hotel with the specified ID was not found.</response>
    /// <response code="500">If an internal server error occurs while updating the hotel.</response>
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateHotel(Guid id,
        [FromBody] JsonPatchDocument<UpdateHotelDto> hotelUpdateDto)
    {
        var hotelDocument = _mapper.Map<JsonPatchDocument<HotelUpdate>>(hotelUpdateDto);

        await _mediator.Send(new UpdateHotelCommand
        {
            Id = id,
            HotelDocument = hotelDocument
        });

        return Ok();
    }
}