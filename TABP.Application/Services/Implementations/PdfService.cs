using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TABP.Application.Queries.Invoices;
using TABP.Application.Services.Interfaces;

namespace TABP.Application.Services.Implementations;

public class PdfService : IPdfService
{
    public async Task<byte[]> GenerateInvoiceAsPdfAsync(EmailInvoiceBody invoice)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        using var stream = new MemoryStream();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);

                page.Margin(2, Unit.Centimetre);
                page.MarginTop(4, Unit.Centimetre);

                page.Header()
                    .Text($"Booking {invoice.BookingId} Invoice").FontSize(20).Bold()
                    .AlignCenter();

                page.Content()
                    .PaddingLeft(1, Unit.Centimetre)
                    .PaddingTop(2, Unit.Centimetre)
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);

                        x.Item().Row(row =>
                        {
                            row.ConstantItem(100).Text("Invoice Number:").Bold();
                            row.RelativeItem().Text(invoice.InvoiceId.ToString());
                        });
                        x.Item().Row(row =>
                        {
                            row.ConstantItem(40).Text("Invoice Date:").Bold();
                            row.RelativeItem().Text(invoice.InvoiceDate.ToString("dd/MM/yyyy"));
                        });

                        x.Item().LineHorizontal(0.5f);

                        x.Item().Row(row =>
                        {
                            row.ConstantItem(100).Text("Payment Method:").Bold();
                            row.RelativeItem().Text(invoice.PaymentMethod);
                        });
                        x.Item().Row(row =>
                        {
                            row.ConstantItem(100).Text("Date of Payment:").Bold();
                            row.RelativeItem().Text(invoice.PaymentDate.ToString("dd/MM/yyyy"));
                        });
                        x.Item().Row(row =>
                        {
                            row.ConstantItem(110).Text("Total Amount Paid:").Bold();
                            row.RelativeItem().Text(invoice.TotalAmount.ToString("C"));
                        });
                        x.Item().LineHorizontal(0.5f);
                    });
            });
        }).GeneratePdf(stream);

        return await Task.FromResult(stream.ToArray());
    }
}