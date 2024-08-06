using TABP.Application.Commands.Bookings;

namespace TABP.Application.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, EmailInvoiceBody invoice);
}