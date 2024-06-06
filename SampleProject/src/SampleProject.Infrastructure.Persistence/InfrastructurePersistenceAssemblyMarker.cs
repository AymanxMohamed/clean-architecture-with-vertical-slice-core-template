using System.Reflection;

namespace SampleProject.Infrastructure.Persistence;

public class InfrastructurePersistenceAssemblyMarker
{
    public static readonly Assembly Assembly = typeof(InfrastructurePersistenceAssemblyMarker).Assembly;
}