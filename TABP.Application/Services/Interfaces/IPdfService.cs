using TABP.Application.Queries.Invoices;

namespace TABP.Application.Services.Interfaces;

public interface IPdfService
{
    Task<byte[]> GenerateInvoiceAsPdfAsync(EmailInvoiceBody invoice);
}