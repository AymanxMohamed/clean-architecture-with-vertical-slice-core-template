using System.Reflection;

namespace Core.Infrastructure.Persistence.SqlServer;

public static class CoreInfrastructurePersistenceSqlServerAssemblyMarker
{
    public static Assembly Assembly => typeof(CoreInfrastructurePersistenceSqlServerAssemblyMarker).Assembly;
}