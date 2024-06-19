using Serilog;

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
            configuration.ReadFrom.Configuration(context.Configuration);
        });
        
        return host;
    }
}