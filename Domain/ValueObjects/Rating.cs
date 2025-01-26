using Domain.Common;

namespace Domain.ValueObjects;

public class Rating : ValueObject
{
    public int Value { get; private set; }

    private Rating(int value)
    {
        Value = value;
    }

    public static Result<Rating> Create(int rating)
    {
        if (rating < 1 || rating > 5)
            return Result.Failure<Rating>("Rating must be between 1 and 5");

        return Result.Success(new Rating(rating));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}