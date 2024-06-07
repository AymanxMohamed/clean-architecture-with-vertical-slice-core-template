using Core.Domain.Aggregates.UserAggregate;

namespace Core.Application.Authentication.Common;

public record AuthenticationResult(
    User User,
    string Token);