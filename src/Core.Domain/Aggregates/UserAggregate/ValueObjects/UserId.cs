﻿using Core.Domain.Common.Models;

namespace Core.Domain.Aggregates.UserAggregate.ValueObjects;

public class UserId : ValueObject
{
    private UserId()
    {
    }

    private UserId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; protected set; }

    public static UserId CreateUnique() => new(Guid.NewGuid());

    public static UserId Create(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}