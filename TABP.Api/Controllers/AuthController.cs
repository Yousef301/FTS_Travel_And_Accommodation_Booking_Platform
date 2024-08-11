using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.Users.Auth;
using TABP.Application.Commands.Users.Register;
using TABP.Web.DTOs.Auth;

namespace TABP.Web.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{v:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)

    {
        var loginCommand = _mapper.Map<LoginCommand>(request);

        var response = await _mediator.Send(loginCommand);

        return Ok(response.Token);
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)

    {
        var loginCommand = _mapper.Map<RegisterCommand>(request);

        await _mediator.Send(loginCommand);

        return NoContent();
    }
}