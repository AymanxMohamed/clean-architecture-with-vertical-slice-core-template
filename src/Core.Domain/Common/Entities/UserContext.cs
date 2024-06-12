using Core.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Core.Domain.Common.Entities;

public class UserContext
{
    private UserContext()
    {
    }

    private UserContext(UserId userId, string firstName, string lastName, string email, string jti)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Jti = jti;
    }
    
    public UserId UserId { get; private set; }
    
    public string FirstName { get; private set; }
    
    public string LastName { get; private set; }
    
    public string Email { get; private set; }
    
    public string Jti { get; private set; }

    public static ErrorOr<UserContext> Create(
        string? userId, 
        string? firstName, 
        string? lastName, 
        string? email, 
        string? jti)
    {
        if (string.IsNullOrWhiteSpace(userId) 
            || string.IsNullOrWhiteSpace(firstName) 
            || string.IsNullOrWhiteSpace(lastName) 
            || string.IsNullOrWhiteSpace(email) 
            || string.IsNullOrWhiteSpace(jti))
        {
            return Errors.Errors.UserContext.NotFound;
        }
        
        if (!Guid.TryParse(userId, out var parsedUserId))
        {
            return Errors.Errors.UserContext.InvalidUserId;
        }

        return new UserContext(UserId.Create(parsedUserId), firstName, lastName, email, jti);
    }
}