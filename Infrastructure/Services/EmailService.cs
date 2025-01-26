using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly SendGridClient _sendGridClient;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public EmailService(
        ILogger<EmailService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _sendGridClient = new SendGridClient(configuration["SendGrid:ApiKey"]);
        _fromEmail = configuration["SendGrid:FromEmail"];
        _fromName = configuration["SendGrid:FromName"];
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var msg = new SendGridMessage
        {
            From = new EmailAddress(_fromEmail, _fromName),
            Subject = subject,
            PlainTextContent = body,
            HtmlContent = body
        };
        msg.AddTo(new EmailAddress(to));

        var response = await _sendGridClient.SendEmailAsync(msg);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to send email to {To}", to);
        }
    }

    public async Task SendEventReminderAsync(Guid userId, Guid eventId)
    {
        // Implementation
    }

    public async Task SendBookingConfirmationAsync(Guid userId, Guid eventId)
    {
        // Implementation
    }
}