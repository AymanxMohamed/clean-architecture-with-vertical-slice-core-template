using Core.Application.Authentication.Common;
using Core.Application.Common.Interfaces.Authentication;
using Core.Application.Common.Users;
using Core.Application.Mediatr.Messages.Commands;
using Core.Domain.Aggregates.UserAggregate;
using Core.Domain.Common.Errors;
using Core.Domain.Services;

namespace Core.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator, 
    IPasswordHasher passwordHasher, 
    IUsersCommandRepository usersCommandRepository,
    IUsersQueryRepository usersQueryRepository) : ICommandHandler<RegisterCommand, AuthenticationResult>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (await usersQueryRepository.ExistsByEmailAsync(command.Email))
        {
            return Errors.User.GenerateDuplicateEmailError(command.Email);
        }
        
        var hashPasswordResult = passwordHasher.HashPassword(command.Password);

        if (hashPasswordResult.IsError)
        {
            return hashPasswordResult.Errors;
        }

        var user = User.Create(
            command.FirstName,
            command.LastName,
            command.Email,
            hashPasswordResult.Value);

        await usersCommandRepository.CreateAsync(user);

        var token = jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}