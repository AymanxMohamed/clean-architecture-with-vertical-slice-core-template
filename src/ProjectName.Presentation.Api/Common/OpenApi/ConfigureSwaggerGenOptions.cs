using Asp.Versioning.ApiExplorer;

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProjectName.Presentation.Api.Common.OpenApi;

public class ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider) : IConfigureNamedOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        ConfigureApiVersioning(options);

        ConfigureJwtSecurity(options);
    }
    
    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private void ConfigureApiVersioning(SwaggerGenOptions options)
    {
        foreach (var versionDescription in provider.ApiVersionDescriptions)
        {
            var openApiInfo = new OpenApiInfo
            {
                Title = $"ProjectName API v{versionDescription.ApiVersion}",
                Version = versionDescription.ApiVersion.ToString(),
                Description = versionDescription.IsDeprecated ? "This API version has been deprecated." : null
            };
            
            options.SwaggerDoc(versionDescription.GroupName, openApiInfo);
        }
    }
    
    private void ConfigureJwtSecurity(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition($"Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme.",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        });
        
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    }
}