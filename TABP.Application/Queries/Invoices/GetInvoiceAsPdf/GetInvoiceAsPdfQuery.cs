using MediatR;

namespace TABP.Application.Queries.Invoices.GetInvoiceAsPdf;

public class GetInvoiceAsPdfQuery : IRequest<byte[]>
{
    public Guid BookingId { get; set; }
}