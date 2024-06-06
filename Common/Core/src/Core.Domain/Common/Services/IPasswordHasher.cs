using ErrorOr;

namespace Core.Domain.Services;

public interface IPasswordHasher
{
    public ErrorOr<string> HashPassword(string password);
    
    bool IsCorrectPassword(string password, string hash);
}