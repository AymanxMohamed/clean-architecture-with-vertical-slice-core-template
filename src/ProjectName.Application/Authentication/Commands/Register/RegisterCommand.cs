using ProjectName.Application.Authentication.Dtos;
using ProjectName.Application.Common.Mediatr.Messages.Commands;

namespace ProjectName.Application.Authentication.Commands.Register;

public record RegisterCommand(string FirstName, string LastName, string Email, string Password) 
    : ICommand<AuthenticationResult>;