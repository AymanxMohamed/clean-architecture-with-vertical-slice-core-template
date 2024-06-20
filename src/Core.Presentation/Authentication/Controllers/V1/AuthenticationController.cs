using Asp.Versioning;

using Core.Application.Authentication.Commands.Register;
using Core.Application.Authentication.Dtos;
using Core.Application.Authentication.Queries.Login;
using Core.Domain.Common.Errors;
using Core.Presentation.Authentication.Dtos;
using Core.Presentation.Common.Controllers;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Presentation.Authentication.Controllers.V1;

[ApiVersion(1, Deprecated = true)]
[AllowAnonymous]
public class AuthenticationController(ISender sender, IMapper mapper) : ApiController(sender, mapper)
{
    [ApiVersion(3)]
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