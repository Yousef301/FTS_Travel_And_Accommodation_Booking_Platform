﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.Users.Auth;
using TABP.Application.Commands.Users.Register;
using TABP.Web.DTOs.Auth;

namespace TABP.Web.Controllers;

[Route("api/auth")]
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
    public async Task<ActionResult> Login(LoginRequest request)

    {
        var loginCommand = _mapper.Map<LoginCommand>(request);

        var response = await _mediator.Send(loginCommand);

        return Ok(response.Token);
    }

    [HttpPost("register")]
    public async Task<ActionResult> Login(RegisterRequest request)

    {
        var loginCommand = _mapper.Map<RegisterCommand>(request);

        await _mediator.Send(loginCommand);

        return NoContent();
    }
}