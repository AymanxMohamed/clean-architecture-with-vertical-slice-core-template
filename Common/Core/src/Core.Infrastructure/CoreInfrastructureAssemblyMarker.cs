using System.Reflection;

namespace Core.Infrastructure;

public class CoreInfrastructureAssemblyMarker
{
    public static readonly Assembly Assembly = typeof(CoreInfrastructureAssemblyMarker).Assembly;
}