﻿using Core.Application.Authentication.Dtos;
using Core.Application.Authentication.Interfaces;
using Core.Application.Authentication.Specifications;
using Core.Application.Common.Mediatr.Messages.Commands;
using Core.Application.Common.Persistence;
using Core.Domain.Aggregates.UserAggregate;
using Core.Domain.Aggregates.UserAggregate.ValueObjects;
using Core.Domain.Common.Errors;
using Core.Domain.Common.Services;

namespace Core.Application.Authentication.Commands.Register;

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