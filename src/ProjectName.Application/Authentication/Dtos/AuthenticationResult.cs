using ProjectName.Domain.Aggregates.UserAggregate;

namespace ProjectName.Application.Authentication.Dtos;

public record AuthenticationResult(
    User User,
    string Token);