using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Queries.Invoices.GetInvoiceAsPdf;
using TABP.Domain.Enums;
using TABP.Web.Services.Interfaces;

namespace TABP.Web.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{v:apiVersion}/user/bookings")]
[Authorize(Roles = nameof(Role.Customer))]
public class InvoicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvoicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{bookingId:guid}/invoice")]
    public async Task<IActionResult> GetInvoiceAsPdf(Guid bookingId)
    {
        var invoice = await _mediator.Send(new GetInvoiceAsPdfQuery
        {
            BookingId = bookingId
        });

        return File(invoice, "application/pdf", "invoice.pdf");
    }
}