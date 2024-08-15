using System.Net;
using System.Net.Mail;
using System.Text;
using TABP.Application.Queries.Invoices;
using TABP.Application.Services.Interfaces;
using TABP.Domain.Exceptions;
using TABP.Domain.Services.Interfaces;

namespace TABP.Application.Services.Implementations;

public class SmtpEmailService : IEmailService
{
    private readonly string _mail;
    private readonly string _password;

    public SmtpEmailService(ISecretsManagerService secretsManagerService)
    {
        var secrets = secretsManagerService.GetSecretAsDictionaryAsync("dev_fts_smtp").Result
                      ?? throw new ArgumentNullException(nameof(secretsManagerService));

        _mail = secrets["Mail"];
        _password = secrets["EmailPassword"];
    }

    public async Task SendEmailAsync(string email, string subject, EmailInvoiceBody invoice)
    {
        try
        {
            var message = new MailMessage
            {
                From = new MailAddress(_mail),
                Subject = subject,
                Body = GetEmailBody(invoice),
                IsBodyHtml = true
            };
            message.To.Add(email);

            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(_mail, _password),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }
        catch (SmtpException smtpEx)
        {
            throw new EmailSendingException("Failed to send email due to an SMTP error.", smtpEx);
        }
        catch (IOException ioEx)
        {
            throw new EmailTemplateException("Failed to send email due to a file read error.", ioEx);
        }
    }

    private string GetEmailBody(EmailInvoiceBody invoice)
    {
        try
        {
            var templatePath = "wwwroot/InvoiceTemplate.html";
            var template = File.ReadAllText(templatePath, Encoding.UTF8);
            var body = template
                .Replace("@Model.InvoiceNumber", invoice.InvoiceId.ToString())
                .Replace("@Model.BookingNumber", invoice.BookingId.ToString())
                .Replace("@Model.PaymentMethod", invoice.PaymentMethod)
                .Replace("@Model.PaymentStatus", invoice.PaymentStatus)
                .Replace("@Model.PaymentDate", invoice.PaymentDate.ToString("dd/MM/yyyy"))
                .Replace("@Model.TotalPrice", invoice.TotalAmount.ToString("C"))
                .Replace("@Model.InvoiceDate", invoice.InvoiceDate.ToString("dd/MM/yyyy"));

            return body;
        }
        catch (Exception ex)
        {
            throw new EmailTemplateException("Failed to read the email template.", ex);
        }
    }
}