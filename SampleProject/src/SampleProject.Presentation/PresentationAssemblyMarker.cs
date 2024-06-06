using System.Reflection;

namespace SampleProject.Presentation;

public class PresentationAssemblyMarker
{
    public static readonly Assembly Assembly = typeof(PresentationAssemblyMarker).Assembly;
}