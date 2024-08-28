using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Commands.Payments.PaymentWebhook;
using TABP.Application.Queries.Payments.GetPayments;
using TABP.Shared.Enums;
using TABP.Web.Helpers.Interfaces;

namespace TABP.Web.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{v:apiVersion}/user/payments")]
[Authorize(Roles = nameof(Role.Customer))]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserIdentity _userIdentity;

    public PaymentsController(IMediator mediator,
        IUserIdentity userIdentity)
    {
        _mediator = mediator;
        _userIdentity = userIdentity;
    }

    /// <summary>
    /// Retrieves a list of payments for the currently authenticated user.
    /// </summary>
    /// <returns>A list of payments for the authenticated user.</returns>
    /// <response code="200">Returns the list of payments for the authenticated user.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">User does not have permission.</response>
    /// <response code="500">If an internal server error occurs while retrieving the payments.</response>
    [HttpGet]
    public async Task<IActionResult> GetPayments()
    {
        var payments = await _mediator.Send(new GetPaymentsQuery
        {
            UserId = _userIdentity.Id
        });

        return Ok(payments);
    }

    /// <summary>
    /// Handles Stripe webhook events.
    /// </summary>
    /// <remarks>
    /// This endpoint processes incoming webhook events from Stripe. It reads the raw event data from the request
    /// body and the Stripe signature from the headers, then processes the event accordingly.
    /// </remarks>
    /// <response code="200">Indicates that the webhook was processed successfully.</response>
    /// <response code="400">If the webhook request is malformed or invalid data is provided.</response>
    /// <response code="500">If an internal server error occurs while processing the webhook.</response>
    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> StripeWebhook()
    {
        await _mediator.Send(new PaymentWebhookCommand
        {
            DataStream = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(),
            Signature = Request.Headers["Stripe-Signature"]
        });

        return Ok();
    }
}