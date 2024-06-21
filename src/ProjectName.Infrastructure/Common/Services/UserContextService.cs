using System.Security.Claims;

using ProjectName.Application.Common.Contexts;
using ProjectName.Domain.Common.Entities;
using ProjectName.Domain.Common.Errors;

using ErrorOr;

using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ProjectName.Infrastructure.Common.Services;

public class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
{
    public ErrorOr<UserContext> GetUserContext()
    {
        var userClaims = httpContextAccessor.HttpContext?.User;

        return UserContext.Create(
            userId: userClaims?.FindFirst(ClaimTypes.NameIdentifier)?.Value, 
            firstName: userClaims?.FindFirst(ClaimTypes.GivenName)?.Value, 
            lastName: userClaims?.FindFirst(ClaimTypes.Surname)?.Value, 
            email: userClaims?.FindFirst(ClaimTypes.Email)?.Value, 
            jti: userClaims?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value);
    }
}