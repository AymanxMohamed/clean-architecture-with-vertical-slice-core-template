using System.Reflection;

namespace Core.Infrastructure.Persistence.Postgres;

public static class CoreInfrastructurePersistencePostgresAssemblyMarker
{
    public static Assembly Assembly => typeof(CoreInfrastructurePersistencePostgresAssemblyMarker).Assembly;
}