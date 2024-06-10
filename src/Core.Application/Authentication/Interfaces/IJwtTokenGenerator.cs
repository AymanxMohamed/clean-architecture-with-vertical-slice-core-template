using Core.Domain.Aggregates.UserAggregate;

namespace Core.Application.Authentication.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}