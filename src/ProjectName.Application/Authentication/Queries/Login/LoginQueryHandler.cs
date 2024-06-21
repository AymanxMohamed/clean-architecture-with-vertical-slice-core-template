using ProjectName.Application.Authentication.Dtos;
using ProjectName.Application.Authentication.Interfaces;
using ProjectName.Application.Authentication.Specifications;
using ProjectName.Application.Common.Mediatr.Messages.Queries;
using ProjectName.Application.Common.Persistence;
using ProjectName.Domain.Aggregates.UserAggregate;
using ProjectName.Domain.Aggregates.UserAggregate.ValueObjects;
using ProjectName.Domain.Common.Errors;
using ProjectName.Domain.Common.Services;

namespace ProjectName.Application.Authentication.Queries.Login;

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