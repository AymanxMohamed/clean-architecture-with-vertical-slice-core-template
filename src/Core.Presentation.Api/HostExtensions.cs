using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Core.Presentation.Api;

public static class HostExtensions
{
    public static ConfigureHostBuilder AddCoreHostConfigurations(this ConfigureHostBuilder host)
    {
        return host.ConfigureSerilog();
    }

    private static ConfigureHostBuilder ConfigureSerilog(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions
                    {
                        IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-" +
                                      $"{context.HostingEnvironment.EnvironmentName.ToLower().Replace('.', '-')}" +
                                      $"-{DateTime.UtcNow:yyyy-MM}"
                    });
        });
        
        return host;
    }
}