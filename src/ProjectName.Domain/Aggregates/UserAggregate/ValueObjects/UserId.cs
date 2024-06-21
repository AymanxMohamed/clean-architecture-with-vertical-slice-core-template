using ProjectName.Domain.Common.Models;

namespace ProjectName.Domain.Aggregates.UserAggregate.ValueObjects;

public class UserId : ValueObject
{
    private UserId()
    {
    }

    private UserId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

    public static UserId CreateUnique() => new(Guid.NewGuid());

    public static UserId Create(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}