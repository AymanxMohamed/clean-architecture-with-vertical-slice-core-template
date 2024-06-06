using System.Reflection;

namespace Core.Infrastructure.Persistence.Common.Services;

internal static class EfCoreConfigurationsAssemblyProvider
{
    private static Assembly? s_efCoreConfigurationsAssembly;

    internal static Assembly GetEfCoreConfigurationsAssembly() => 
        s_efCoreConfigurationsAssembly 
        ?? throw new ArgumentNullException(
            $"EfCoreConfigurationAssembly", 
            "EfCoreConfiguration Assembly must be provided");
    
    internal static void SetEfCoreConfigurationsAssembly(Assembly configurationAssembly) => 
        s_efCoreConfigurationsAssembly = configurationAssembly;
}