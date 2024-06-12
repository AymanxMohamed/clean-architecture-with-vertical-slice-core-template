using System.Security.Claims;

using Core.Application.Common.Contexts;
using Core.Domain.Common.Entities;
using Core.Domain.Common.Errors;

using ErrorOr;

using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Core.Infrastructure.Common.Services;

public class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
{
    public ErrorOr<UserContext> GetUserContext()
    {
        var userClaims = httpContextAccessor.HttpContext?.User;

        return UserContext.Create(
            userId: userClaims?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value, 
            firstName: userClaims?.FindFirst(JwtRegisteredClaimNames.GivenName)?.Value, 
            lastName: userClaims?.FindFirst(JwtRegisteredClaimNames.FamilyName)?.Value, 
            email: userClaims?.FindFirst(JwtRegisteredClaimNames.Email)?.Value, 
            jti: userClaims?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value);
    }
}