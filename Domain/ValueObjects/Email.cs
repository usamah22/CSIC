using Domain.Common;

namespace Domain.ValueObjects;

public class Email : ValueObject
{
    public string Value { get; private set; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<Email>("Email cannot be empty");

        if (!email.EndsWith("@aston.ac.uk", StringComparison.OrdinalIgnoreCase))
            return Result.Failure<Email>("Email must be an Aston University email");

        return Result.Success(new Email(email));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToLower();
    }
}