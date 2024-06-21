namespace ProjectName.Domain.Common.Services;

public interface IPasswordHasher
{
    public ErrorOr<string> HashPassword(string password);
    
    bool IsCorrectPassword(string password, string hash);
}