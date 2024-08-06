﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Queries.Payments.GetPayments;
using TABP.Web.Enums;
using TABP.Web.Services.Interfaces;

namespace TABP.Web.Controllers;

[ApiController]
[Route("api/user/payments")]
[Authorize(Roles = nameof(Role.Customer))]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public PaymentsController(IMediator mediator, IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetPayments()
    {
        var payments = await _mediator.Send(new GetPaymentsQuery
        {
            UserId = _userContext.Id
        });

        return Ok(payments);
    }
}