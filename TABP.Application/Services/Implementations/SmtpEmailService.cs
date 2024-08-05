using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using TABP.Application.Services.Interfaces;

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

    public Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(_mail, _password),
            EnableSsl = true
        };

        return client.SendMailAsync(
            new MailMessage(from: _mail,
                to: email,
                subject,
                message));
    }
}