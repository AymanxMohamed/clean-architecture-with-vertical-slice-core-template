using Core.Application.Authentication.Dtos;
using Core.Application.Authentication.Interfaces;
using Core.Application.Authentication.Specifications;
using Core.Application.Common.Mediatr.Messages.Queries;
using Core.Application.Common.Persistence;
using Core.Domain.Aggregates.UserAggregate;
using Core.Domain.Aggregates.UserAggregate.ValueObjects;
using Core.Domain.Common.Errors;
using Core.Domain.Common.Services;

namespace Core.Application.Authentication.Queries.Login;

public class LoginQueryHandler(
    ICachedGenericRepository<User, UserId> userGenericRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator) : IQueryHandler<LoginQuery, AuthenticationResult>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var user = await userGenericRepository.GetFirstOrDefault(new UserByEmailSpecification(query.Email), cancellationToken);

        return user is null || !user.IsCorrectPasswordHash(query.Password, passwordHasher)
            ? Errors.Authentication.InvalidCredentials
            : new AuthenticationResult(user, jwtTokenGenerator.GenerateToken(user));
    }
}