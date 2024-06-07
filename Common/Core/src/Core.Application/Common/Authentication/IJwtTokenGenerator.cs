using Core.Domain.Aggregates.UserAggregate;

namespace Core.Application.Common.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}