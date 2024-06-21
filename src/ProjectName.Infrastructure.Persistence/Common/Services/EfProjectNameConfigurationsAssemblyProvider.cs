using System.Reflection;

namespace ProjectName.Infrastructure.Persistence.Common.Services;

internal static class EfProjectNameConfigurationsAssemblyProvider
{
    private static Assembly? s_efProjectNameConfigurationsAssembly;

    internal static Assembly GetEfProjectNameConfigurationsAssembly() => 
        s_efProjectNameConfigurationsAssembly 
        ?? throw new ArgumentNullException(
            $"EfProjectNameConfigurationAssembly", 
            "EfProjectNameConfiguration Assembly must be provided");
    
    internal static void SetEfProjectNameConfigurationsAssembly(Assembly configurationAssembly) => 
        s_efProjectNameConfigurationsAssembly = configurationAssembly;
}