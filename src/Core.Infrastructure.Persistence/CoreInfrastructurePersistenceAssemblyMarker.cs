using System.Reflection;

namespace Core.Infrastructure.Persistence;

public class CoreInfrastructurePersistenceAssemblyMarker
{
    public static readonly Assembly Assembly = typeof(CoreInfrastructurePersistenceAssemblyMarker).Assembly;
}