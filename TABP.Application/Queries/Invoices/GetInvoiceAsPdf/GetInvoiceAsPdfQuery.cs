using MediatR;

namespace TABP.Application.Queries.Invoices.GetInvoiceAsPdf;

public class GetInvoiceAsPdfQuery : IRequest<byte[]>
{
    public Guid UserId { get; set; }
    public Guid BookingId { get; set; }
}