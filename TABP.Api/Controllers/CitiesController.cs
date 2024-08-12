using System.Text.Json;
using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using TABP.Application.Commands.Cities.CreateCity;
using TABP.Application.Commands.Cities.DeleteCity;
using TABP.Application.Commands.Cities.UpdateCity;
using TABP.Application.Queries.Cities;
using TABP.Application.Queries.Cities.GetCitiesForAdmin;
using TABP.Application.Queries.Cities.GetTrendingCities;
using TABP.Domain.Enums;
using TABP.Domain.Extensions;
using TABP.Web.DTOs;
using TABP.Web.DTOs.Cities;
using TABP.Web.Extensions;

namespace TABP.Web.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{v:apiVersion}/cities")]
[Authorize(Roles = nameof(Role.Admin))]
public class CitiesController : ControllerBase
{
    private const string CacheKey = "TrendingCities";

    private readonly IDistributedCache _distributedCache;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CitiesController(IMediator mediator, IMapper mapper, IDistributedCache distributedCache)
    {
        _mediator = mediator;
        _mapper = mapper;
        _distributedCache = distributedCache;
    }

    /// <summary>
    /// Retrieves a list of trending cities.
    /// </summary>
    /// <returns>A list of trending cities.</returns>
    /// <response code="200">Returns a list of trending cities.</response>
    /// <response code="500">If an internal server error occurs while retrieving the trending cities.</response>
    [HttpGet("trending")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTrendingCities()
    {
        var cachedCities = await _distributedCache.GetStringAsync(CacheKey);

        if (string.IsNullOrEmpty(cachedCities))
        {
            var trendingCities = await _mediator.Send(
                new GetTrendingCitiesQuery());

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            await _distributedCache.SetStringAsync(CacheKey, JsonSerializer.Serialize(trendingCities), cacheOptions);
            return Ok(trendingCities);
        }

        var cities = JsonSerializer.Deserialize<List<TrendingCitiesResponse>>(cachedCities);

        return Ok(cities);
    }

    /// <summary>
    /// Retrieves a list of cities for admin users with optional filtering and pagination.
    /// </summary>
    /// <param name="filterParameters">The parameters used to filter and paginate the list of cities.</param>
    /// <returns>A paginated list of cities.</returns>
    /// <response code="200">Returns a paginated list of cities.</response>
    /// <response code="400">If the provided filter parameters are invalid.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to access this resource.</response>
    /// <response code="500">If an internal server error occurs while retrieving the cities.</response>
    [HttpGet]
    public async Task<IActionResult> GetCitiesForAdmin([FromQuery] FilterParameters filterParameters)
    {
        var query = _mapper.Map<GetCitiesForAdminQuery>(filterParameters);

        var cities = await _mediator.Send(query);

        var metadata = cities.ToMetadata();

        Response.AddPaginationMetadata(metadata, Request);


        return Ok(cities.Items);
    }

    /// <summary>
    /// Creates a new city.
    /// </summary>
    /// <param name="cityCreateDto">The details required to create a new city.</param>
    /// <response code="201">The city was successfully created.</response>
    /// <response code="400">If the request contains invalid data or is missing required fields.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to create a city.</response>
    /// <response code="409">If a city with the same name and country already exists.</response>
    /// <response code="500">If an internal server error occurs while creating the city.</response>
    [HttpPost]
    public async Task<IActionResult> CreateCity([FromBody] CreateCityDto cityCreateDto)
    {
        var command = _mapper.Map<CreateCityCommand>(cityCreateDto);

        await _mediator.Send(command);

        return Created();
    }

    /// <summary>
    /// Deletes a city by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the city to delete.</param>
    /// <response code="204">The city was successfully deleted.</response>
    /// <response code="400">If the provided ID is not valid or malformed.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to delete the city.</response>
    /// <response code="404">The city with the specified ID was not found.</response>
    /// <response code="500">If an internal server error occurs while deleting the city.</response>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCity(Guid id)
    {
        await _mediator.Send(new DeleteCityCommand { Id = id });

        return NoContent();
    }

    /// <summary>
    /// Updates an existing city by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the city to update.</param>
    /// <param name="cityUpdateDto">The patch document containing the updates to apply to the city.</param>
    /// <response code="200">The city was successfully updated.</response>
    /// <response code="400">If the request contains invalid data or is missing required fields.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission to update the city.</response>
    /// <response code="404">The city with the specified ID was not found.</response>
    /// <response code="500">If an internal server error occurs while updating the city.</response>
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateCity(Guid id,
        [FromBody] JsonPatchDocument<UpdateCityDto> cityUpdateDto)
    {
        var cityDocument = _mapper.Map<JsonPatchDocument<CityUpdate>>(cityUpdateDto);

        await _mediator.Send(new UpdateCityCommand { Id = id, CityDocument = cityDocument });

        return Ok();
    }
}