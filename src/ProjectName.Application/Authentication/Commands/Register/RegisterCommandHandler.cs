using ProjectName.Application.Authentication.Dtos;
using ProjectName.Application.Authentication.Interfaces;
using ProjectName.Application.Authentication.Specifications;
using ProjectName.Application.Common.Mediatr.Messages.Commands;
using ProjectName.Application.Common.Persistence;
using ProjectName.Domain.Aggregates.UserAggregate;
using ProjectName.Domain.Aggregates.UserAggregate.ValueObjects;
using ProjectName.Domain.Common.Errors;
using ProjectName.Domain.Common.Services;

namespace ProjectName.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator, 
    IPasswordHasher passwordHasher, 
    ICachedGenericRepository<User, UserId> userGenericRepository) : ICommandHandler<RegisterCommand, AuthenticationResult>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (await userGenericRepository.CheckExistAsync(new UserByEmailSpecification(command.Email), cancellationToken))
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
        
        await userGenericRepository.AddAsync(user, cancellationToken);

        var token = jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}