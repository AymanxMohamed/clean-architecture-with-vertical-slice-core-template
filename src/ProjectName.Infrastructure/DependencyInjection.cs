using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectName.Application.Authentication.Interfaces;
using ProjectName.Application.Common.Contexts;
using ProjectName.Application.Common.Services;
using ProjectName.Domain.Common.Services;
using ProjectName.Infrastructure.Authentication.PasswordHasher;
using ProjectName.Infrastructure.Authentication.TokenGenerator;
using ProjectName.Infrastructure.Common.Services;
using ProjectName.Infrastructure.Common.Services.Caching;
using ProjectName.Infrastructure.Common.Services.Email;
using ProjectName.Infrastructure.Services;

namespace ProjectName.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectNameInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddCommonServices()
            .AddAuth(configuration)
            .AddUserContext()
            .AddEmailServices(configuration)
            .AddCaching(configuration);
    }

    private static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        return services;
    }
    
    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings).Value);

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            });
        return services;
    }

    private static IServiceCollection AddUserContext(this IServiceCollection services)
    {
        return services.AddTransient<IUserContextService, UserContextService>();
    }
    
    private static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
    {
        var emailServiceOptions = configuration
                                      .GetSection(nameof(EmailServerSettings))
                                      .Get<EmailServerSettings>() 
                                  ?? throw new ApplicationException(
                                      message: "Email service Settings must be added to appsettings.json file");
        return services
            .AddSingleton(emailServiceOptions)
            .AddScoped<IEmailService, EmailService>();
    }

    private static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var cachingSettings = configuration
                                  .GetSection(CachingSettings.SectionName)
                                  .Get<CachingSettings>() ?? new CachingSettings();

        if (cachingSettings is { RedisCacheEnabled: true })
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cachingSettings.RedisServerUrl;
            });
        }
        else
        {
            services.AddDistributedMemoryCache();
        }
        
        services.AddSingleton(cachingSettings);
        services.AddSingleton<ICachingService, CachingService>();

        return services;
    }
}