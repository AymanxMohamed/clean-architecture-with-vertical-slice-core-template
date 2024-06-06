using System.Reflection;

namespace Core.Application;

public class CoreApplicationAssemblyMarker
{
    public static readonly Assembly Assembly = typeof(CoreApplicationAssemblyMarker).Assembly;
}