using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using ProjectName.Application.Authentication.Interfaces;
using ProjectName.Domain.Aggregates.UserAggregate;
using ProjectName.Domain.Common.Services;

using Microsoft.IdentityModel.Tokens;

namespace ProjectName.Infrastructure.Authentication.TokenGenerator;

public class JwtTokenGenerator(IDateTimeProvider dateTimeProvider, JwtSettings jwtSettings) : IJwtTokenGenerator
{
    public string GenerateToken(User user)
    {
        var signingCredentials = new SigningCredentials(
            key: new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            algorithm: SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var securityToken = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            expires: dateTimeProvider.UtcNow.AddMinutes(jwtSettings.ExpiryMinutes),
            claims: claims,
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}