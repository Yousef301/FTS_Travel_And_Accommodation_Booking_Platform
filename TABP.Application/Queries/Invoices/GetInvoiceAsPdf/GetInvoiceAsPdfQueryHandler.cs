﻿using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Queries.Invoices.GetInvoiceAsPdf;

public class GetInvoiceAsPdfQueryHandler : IRequestHandler<GetInvoiceAsPdfQuery, byte[]>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IPdfService _pdfService;

    public GetInvoiceAsPdfQueryHandler(IInvoiceRepository invoiceRepository, IPdfService pdfService)
    {
        _invoiceRepository = invoiceRepository;
        _pdfService = pdfService;
    }

    public async Task<byte[]> Handle(GetInvoiceAsPdfQuery request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetByBookingIdAsync(request.BookingId);

        if (invoice == null)
        {
            throw new NotImplementedException("Invoice not found...");
        }

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