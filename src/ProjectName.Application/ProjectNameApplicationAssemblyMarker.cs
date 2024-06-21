using System.Reflection;

namespace ProjectName.Application;

public class ProjectNameApplicationAssemblyMarker
{
    public static readonly Assembly Assembly = typeof(ProjectNameApplicationAssemblyMarker).Assembly;
}