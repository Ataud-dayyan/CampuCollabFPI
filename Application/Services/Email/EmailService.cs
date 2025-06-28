using Application.Services.Email;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
    {
        // Validate input first
        if (string.IsNullOrEmpty(toEmail))
            throw new ArgumentNullException(nameof(toEmail), "Recipient email cannot be null");

        var smtpHost = _config["Mailtrap:Host"];
        var smtpPort = int.Parse(_config["Mailtrap:Port"]);
        var fromEmail = _config["Mailtrap:Email"];
        var userName = _config["Mailtrap:UserName"];  // Use UserName for auth
        var password = _config["Mailtrap:Password"];

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(userName, password), // Fixed: Use UserName
            EnableSsl = true
        };

        using var message = new MailMessage(fromEmail, toEmail, subject, htmlContent)
        {
            IsBodyHtml = true
        };

        try
        {
            await client.SendMailAsync(message);
        }
        catch (SmtpException ex)
        {
            // Log the specific SMTP error for debugging
            throw new InvalidOperationException($"Failed to send email: {ex.Message}", ex);
        }
    }
}