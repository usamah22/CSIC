namespace Domain.Exceptions;

public class EventBookingException : DomainException
{
    public EventBookingException(string message) : base(message)
    {
    }
}