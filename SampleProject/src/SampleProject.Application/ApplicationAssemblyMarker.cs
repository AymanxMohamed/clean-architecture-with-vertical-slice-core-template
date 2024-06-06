using System.Reflection;

namespace SampleProject.Application;

public class ApplicationAssemblyMarker
{
    public static readonly Assembly Assembly = typeof(ApplicationAssemblyMarker).Assembly;
}