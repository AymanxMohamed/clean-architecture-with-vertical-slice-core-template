﻿using ProjectName.Presentation.Api.Common.Settings.Serilog;

using Serilog;

namespace ProjectName.Presentation.Api;

public static class HostExtensions
{
    public static ConfigureHostBuilder AddProjectNameHostConfigurations(this ConfigureHostBuilder host)
    {
        return host.ConfigureSerilog();
    }

    private static ConfigureHostBuilder ConfigureSerilog(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, configuration) =>
        {
            var serilogSettings = context.Configuration.GetSection(SerilogSettings.SectionName)
                .Get<SerilogSettings>() ?? throw new NotSupportedException(
                message: "you have to add serilog section in the configuration files");

            var elasticSearchSinkOptions = serilogSettings.GetElasticSearchSinkOptions(context);
            
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .WriteTo.Elasticsearch(elasticSearchSinkOptions);
        });
        
        return host;
    }
}