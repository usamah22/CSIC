namespace Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendEventReminderAsync(Guid userId, Guid eventId);
    Task SendBookingConfirmationAsync(Guid userId, Guid eventId);
}