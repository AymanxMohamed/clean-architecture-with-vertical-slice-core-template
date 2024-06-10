﻿using Core.Application.Authentication.Dtos;
using Core.Application.Common.Mediatr.Messages.Commands;

namespace Core.Application.Authentication.Commands.Register;

public record RegisterCommand(string FirstName, string LastName, string Email, string Password) 
    : ICommand<AuthenticationResult>;