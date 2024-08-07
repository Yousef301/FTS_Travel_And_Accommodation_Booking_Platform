using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Queries.Invoices.GetInvoiceAsPdf;
using TABP.Domain.Enums;
using TABP.Web.Services.Interfaces;

namespace TABP.Web.Controllers;

[ApiController]
[Route("api/user/bookings")]
[Authorize(Roles = nameof(Role.Customer))]
public class InvoicesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public InvoicesController(IUserContext userContext, IMediator mediator)
    {
        _mediator = mediator;
        _userContext = userContext;
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