using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="request">The login request containing the username and password.</param>
    /// <returns>A JWT token if the login is successful.</returns>
    /// <response code="200">Returns the JWT token if authentication is successful.</response>
    /// <response code="400">If the login request is invalid (missing required fields).</response>
    /// <response code="401">If the login credentials are invalid (incorrect username or password).</response>
    /// <response code="500">If an internal server error occurs during authentication.</response>
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)

    {
        var loginCommand = _mapper.Map<LoginCommand>(request);

        var response = await _mediator.Send(loginCommand);

        Log.Information("User {Username} logged in", request.Username);

        return Ok(response.Token);
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="request">The registration request containing user details such as username, password, etc.</param>
    /// <response code="201">The user was successfully registered.</response>
    /// <response code="400">If the registration request is invalid or contains errors (e.g., missing required fields, invalid data).</response>
    /// <response code="409">If a user with the same username or email already exists.</response>
    /// <response code="500">If an internal server error occurs during registration.</response>
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)

    {
        var loginCommand = _mapper.Map<RegisterCommand>(request);

        await _mediator.Send(loginCommand);

        return Created();
    }
}