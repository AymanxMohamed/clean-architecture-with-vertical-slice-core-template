﻿using ProjectName.Application.Authentication.Commands.Register;
using ProjectName.Application.Authentication.Dtos;
using ProjectName.Application.Authentication.Queries.Login;
using ProjectName.Domain.Common.Errors;
using ProjectName.Presentation.Authentication.Dtos;
using ProjectName.Presentation.Common.Controllers;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectName.Presentation.Authentication.Controllers;

[AllowAnonymous]
public class AuthenticationController(ISender sender, IMapper mapper) : ApiController(sender, mapper)
{
    [HttpPost("register")]
    [ProducesResponseType<AuthenticationResponse>(statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        
        ErrorOr<AuthenticationResult> authResult = await _sender.Send(command);

        return authResult.Match(
            onValue: result => Ok(_mapper.Map<AuthenticationResponse>(result)),
            onError: Problem);
    }

    [HttpPost("login")]
    [ProducesResponseType<AuthenticationResponse>(statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request);
        ErrorOr<AuthenticationResult> authResult = await _sender.Send(query);
        
        if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(
                detail: authResult.FirstError.Description,
                statusCode: StatusCodes.Status401Unauthorized);
        }

        return authResult.Match(
            onValue: result => Ok(_mapper.Map<AuthenticationResponse>(result)),
            onError: Problem);
    }
}