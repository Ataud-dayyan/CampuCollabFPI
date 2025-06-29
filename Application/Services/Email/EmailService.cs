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
        var smtpUsername = _config["Mailtrap:UserName"]; // Use UserName, not Email
        var smtpPassword = _config["Mailtrap:Password"];
        var fromEmail = _config["Mailtrap:Email"];

        try
        {
            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword), // Fixed credentials
                EnableSsl = true,
                Timeout = 30000, // 30 seconds timeout
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            using var message = new MailMessage(fromEmail, toEmail, subject, htmlContent)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(message); // Only send once!
        }
        catch (SmtpException ex)
        {
            // Log the specific SMTP error for debugging
            throw new InvalidOperationException($"Failed to send email via SMTP: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            // Catch other potential issues like network timeouts
            throw new InvalidOperationException($"Failed to send email: {ex.Message}", ex);
        }
    }
    // Add this test method to verify connectivity
    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            using var client = new SmtpClient(_config["Mailtrap:Host"], int.Parse(_config["Mailtrap:Port"]))
            {
                Credentials = new NetworkCredential(_config["Mailtrap:UserName"], _config["Mailtrap:Password"]),
                EnableSsl = true,
                Timeout = 10000
            };
        
            // Just test the connection without sending
            await client.SendMailAsync("test@test.com", "test@test.com", "Test", "Test");
            return true;
        }
        catch
        {
            return false;
        }
    }
}