using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Queries.Hotels.GetRecentlyVisitedHotels;
using TABP.Domain.Enums;
using TABP.Web.Services.Interfaces;

namespace TABP.Web.Controllers;

[ApiController]
[Route("api/user")]
[Authorize(Roles = nameof(Role.Customer))]
public class UsersController : ControllerBase
{
    private readonly IUserContext _userContext;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UsersController(IUserContext userContext, IMediator mediator, IMapper mapper)
    {
        _userContext = userContext;
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("recently-visited")]
    public async Task<IActionResult> GetRecentlyVisitedHotels([FromQuery] int count = 5)
    {
        var recentlyVisitedHotels = await _mediator.Send(
            new GetRecentlyVisitedHotelsQuery
            {
                UserId = _userContext.Id,
                Count = count
            });

        return Ok(recentlyVisitedHotels);
    }
}