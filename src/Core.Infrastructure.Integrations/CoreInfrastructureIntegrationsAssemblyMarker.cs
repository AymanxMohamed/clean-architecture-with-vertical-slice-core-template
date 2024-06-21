using System.Reflection;

namespace Core.Infrastructure.Integrations;

public class CoreInfrastructureIntegrationsAssemblyMarker
{
    public static readonly Assembly Assembly = typeof(CoreInfrastructureIntegrationsAssemblyMarker).Assembly;
}