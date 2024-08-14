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
[Route("api/v{v:apiVersion}/user/bookings/{bookingId:guid}")]
[Authorize(Roles = nameof(Role.Customer))]
public class InvoicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvoicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves the invoice for a specific booking as a PDF file.
    /// </summary>
    /// <param name="bookingId">The ID of the booking for which to retrieve the invoice.</param>
    /// <returns>The invoice as PDF file for the specified booking ID.</returns>
    /// <response code="200">The invoice PDF was successfully retrieved and is returned as a file.</response>
    /// <response code="404">If no invoice is found for the specified booking ID.</response>
    /// <response code="500">If an internal server error occurs while generating or retrieving the invoice.</response>
    [HttpGet("invoice")]
    public async Task<IActionResult> GetInvoiceAsPdf(Guid bookingId)
    {
        var invoice = await _mediator.Send(new GetInvoiceAsPdfQuery
        {
            BookingId = bookingId
        });

        return File(invoice, "application/pdf", "invoice.pdf");
    }
}