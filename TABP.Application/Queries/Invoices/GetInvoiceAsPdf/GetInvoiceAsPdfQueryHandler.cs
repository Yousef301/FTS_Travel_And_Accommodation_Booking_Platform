using AutoMapper;
using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Shared.Exceptions;

namespace TABP.Application.Queries.Invoices.GetInvoiceAsPdf;

public class GetInvoiceAsPdfQueryHandler : IRequestHandler<GetInvoiceAsPdfQuery, byte[]>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IPdfService _pdfService;
    private readonly IMapper _mapper;

    public GetInvoiceAsPdfQueryHandler(IInvoiceRepository invoiceRepository,
        IPdfService pdfService,
        IMapper mapper)
    {
        _invoiceRepository = invoiceRepository;
        _pdfService = pdfService;
        _mapper = mapper;
    }

    public async Task<byte[]> Handle(GetInvoiceAsPdfQuery request,
        CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetByBookingIdAsync(request.BookingId) ??
                      throw new NotFoundException($"Invoice with booking", request.BookingId);

        if (invoice.Booking.UserId != request.UserId)
            throw new UnauthorizedAccessException("You are not authorized to access this invoice.");

        var invoicePdf = await _pdfService.GenerateInvoiceAsPdfAsync(_mapper.Map<EmailInvoiceBody>(invoice));

        return invoicePdf;
    }
}