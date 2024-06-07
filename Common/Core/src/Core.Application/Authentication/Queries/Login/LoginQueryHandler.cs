using Core.Application.Authentication.Common;
using Core.Application.Common.Authentication;
using Core.Application.Common.Mediatr.Messages.Queries;
using Core.Application.Common.Users;
using Core.Domain.Common.Errors;
using Core.Domain.Services;

namespace Core.Application.Authentication.Queries.Login;

public class LoginQueryHandler(
    IUsersQueryRepository usersQueryRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator) : IQueryHandler<LoginQuery, AuthenticationResult>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var user = await usersQueryRepository.GetByEmailAsync(query.Email);

        return user is null || !user.IsCorrectPasswordHash(query.Password, passwordHasher)
            ? Errors.Authentication.InvalidCredentials
            : new AuthenticationResult(user, jwtTokenGenerator.GenerateToken(user));
    }
}