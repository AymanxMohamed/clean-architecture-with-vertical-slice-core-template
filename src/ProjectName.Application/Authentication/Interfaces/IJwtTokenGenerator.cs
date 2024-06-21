using ProjectName.Domain.Aggregates.UserAggregate;

namespace ProjectName.Application.Authentication.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}