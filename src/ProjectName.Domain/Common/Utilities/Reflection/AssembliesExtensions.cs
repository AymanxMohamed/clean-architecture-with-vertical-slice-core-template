using System.Reflection;

namespace ProjectName.Domain.Common.Utilities.Reflection;

public static class AssembliesExtensions
{
    public static IEnumerable<Type> GetTypesImplementingInterface<TInterface>(this IEnumerable<Assembly> assemblies) =>
        assemblies
            .SelectMany(x => x.GetTypes())
            .Where(t => typeof(TInterface).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });
}