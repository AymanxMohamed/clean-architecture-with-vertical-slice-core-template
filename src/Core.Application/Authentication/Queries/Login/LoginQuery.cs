﻿using Core.Application.Authentication.Dtos;
using Core.Application.Common.Mediatr.Messages.Queries;

namespace Core.Application.Authentication.Queries.Login;

public record LoginQuery(string Email, string Password) : IQuery<AuthenticationResult>;
