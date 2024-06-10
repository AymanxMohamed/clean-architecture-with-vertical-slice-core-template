using Core.Domain.Aggregates.UserAggregate;

namespace Core.Application.Authentication.Dtos;

public record AuthenticationResult(
    User User,
    string Token);