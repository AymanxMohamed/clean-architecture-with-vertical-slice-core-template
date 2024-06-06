using System.Reflection;

namespace SampleProject.Infrastructure;

public class InfrastructureAssemblyMarker
{
    public static readonly Assembly Assembly = typeof(InfrastructureAssemblyMarker).Assembly;
}