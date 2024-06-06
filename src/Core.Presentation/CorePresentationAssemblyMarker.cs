using System.Reflection;

namespace Core.Presentation;

public class CorePresentationAssemblyMarker
{
    public static readonly Assembly Assembly = typeof(CorePresentationAssemblyMarker).Assembly;
}