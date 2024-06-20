﻿using Core.Domain.Aggregates.UserAggregate.Events;
using Core.Domain.Aggregates.UserAggregate.ValueObjects;
using Core.Domain.Common.Models;
using Core.Domain.Common.Services;

namespace Core.Domain.Aggregates.UserAggregate;

public class User : AuditableEntity<UserId>
{
    private readonly string _passwordHash = null!;
    
    private User()
    {
    }

    private User(
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        UserId? id = null)
        : base(id ?? UserId.CreateUnique())
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        _passwordHash = passwordHash;
        
        AddDomainEvent(new UserCreatedDomainEvent(Id));
    }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Email { get; init; }

    public static User Create(
        string firstName,
        string lastName,
        string email,
        string passwordHash)
    {
        return new User(
            firstName,
            lastName,
            email,
            passwordHash);
    }
    
    public bool IsCorrectPasswordHash(string password, IPasswordHasher passwordHasher) => 
        passwordHasher.IsCorrectPassword(password, _passwordHash);
}