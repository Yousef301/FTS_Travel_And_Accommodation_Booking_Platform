using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Queries.Invoices.GetInvoiceAsPdf;

public class GetInvoiceAsPdfQueryHandler : IRequestHandler<GetInvoiceAsPdfQuery, byte[]>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IPdfService _pdfService;

    public GetInvoiceAsPdfQueryHandler(IInvoiceRepository invoiceRepository,
        IPdfService pdfService)
    {
        _invoiceRepository = invoiceRepository;
        _pdfService = pdfService;
    }

    public async Task<byte[]> Handle(GetInvoiceAsPdfQuery request,
        CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetByBookingIdAsync(request.BookingId) ??
                      throw new NotFoundException($"Invoice with booking", request.BookingId);

        if (invoice.Booking.UserId != request.UserId)
            throw new UnauthorizedAccessException("You are not authorized to access this invoice.");

        var invoicePdf = await _pdfService.GenerateInvoiceAsPdfAsync(new EmailInvoiceBody
        {
            InvoiceId = invoice.Id,
            BookingId = invoice.BookingId,
            PaymentMethod = invoice.Booking.PaymentMethod.ToString(),
            PaymentStatus = invoice.PaymentStatus.ToString(),
            PaymentDate = invoice.InvoiceDate,
            InvoiceDate = invoice.InvoiceDate,
            TotalAmount = invoice.TotalPrice
        });

        return invoicePdf;
    }
}