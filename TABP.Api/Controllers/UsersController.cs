using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Queries.Hotels.GetRecentlyVisitedHotels;
using TABP.Shared.Enums;
using TABP.Web.Helpers.Interfaces;

namespace TABP.Web.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{v:apiVersion}/user")]
[Authorize(Roles = nameof(Role.Customer))]
public class UsersController : ControllerBase
{
    private readonly IUserIdentity _userIdentity;
    private readonly IMediator _mediator;

    public UsersController(IUserIdentity userIdentity,
        IMediator mediator)
    {
        _userIdentity = userIdentity;
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves a list of recently visited hotels for the authenticated user.
    /// </summary>
    /// <param name="count">The number of recently visited hotels to return. Defaults to 5.</param>
    /// <returns>A list of recently visited hotels for the authenticated user.</returns>
    /// <response code="200">The list of recently visited hotels was successfully retrieved.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User does not have permission.</response>
    /// <response code="500">If an internal server error occurs while retrieving the list of hotels.</response>
    [HttpGet("recently-visited")]
    public async Task<IActionResult> GetRecentlyVisitedHotels([FromQuery] int count = 5)
    {
        var recentlyVisitedHotels = await _mediator.Send(
            new GetRecentlyVisitedHotelsQuery
            {
                UserId = _userIdentity.Id,
                Count = count
            });

        return Ok(recentlyVisitedHotels);
    }
}