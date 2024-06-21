using ProjectName.Domain.Aggregates.UserAggregate.Events;
using ProjectName.Domain.Aggregates.UserAggregate.ValueObjects;
using ProjectName.Domain.Common.Models;
using ProjectName.Domain.Common.Services;

namespace ProjectName.Domain.Aggregates.UserAggregate;

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

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string Email { get; private set; }

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