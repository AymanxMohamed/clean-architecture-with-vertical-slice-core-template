using System.Reflection;

namespace ProjectName.Infrastructure;

public class ProjectNameInfrastructureAssemblyMarker
{
    public static readonly Assembly Assembly = typeof(ProjectNameInfrastructureAssemblyMarker).Assembly;
}