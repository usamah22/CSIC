namespace Domain.Exceptions;

public class InvalidEmailException : DomainException
{
    public InvalidEmailException(string message) : base(message)
    {
    }
}