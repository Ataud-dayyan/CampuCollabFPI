using Application.Services.Email;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Net;
using System.Net.Mail;
using System.Text.Json;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
    {
        var smtpHost = _config["Mailtrap:Host"];
        var smtpPort = int.Parse(_config["Mailtrap:Port"]);
        var fromEmail = _config["Mailtrap:Email"];
        var password = _config["Mailtrap:Password"];

        var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(fromEmail, password),
            EnableSsl = true
        };

        var message = new MailMessage(fromEmail, fromEmail, subject, htmlContent)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(message);

    }
}