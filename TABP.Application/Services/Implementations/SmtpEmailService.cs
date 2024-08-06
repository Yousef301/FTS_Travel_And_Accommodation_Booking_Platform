using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TABP.Application.Commands.Bookings;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;

namespace TABP.Application.Services.Implementations;

public class SmtpEmailService : IEmailService
{
    private readonly string _mail;
    private readonly string _password;

    public SmtpEmailService(IConfiguration configuration)
    {
        _mail = configuration["Mail"] ?? throw new ArgumentNullException();
        _password = configuration["EmailPassword"] ?? throw new ArgumentNullException();
    }

    public Task SendEmailAsync(string email, string subject, EmailInvoiceBody invoice)
    {
        var message = new MailMessage
        {
            From = new MailAddress(_mail),
            Subject = subject,
            Body = GetEmailBody(invoice),
            IsBodyHtml = true
        };
        message.To.Add(email);


        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(_mail, _password),
            EnableSsl = true
        };

        return client.SendMailAsync(message);
    }

    private string GetEmailBody(EmailInvoiceBody invoice)
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
}