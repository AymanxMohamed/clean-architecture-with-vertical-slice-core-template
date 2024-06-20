using Core.Infrastructure.Persistence.Common.Settings;
using Core.Infrastructure.Persistence.Postgres;
using Core.Infrastructure.Persistence.SqlServer;

using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence.Common.Extensions;

public static class DbContextOptionsBuilderExtensions
{
    public static void ConfigureFromDatabaseConfigurations(
        this DbContextOptionsBuilder optionsBuilder, 
        DatabaseConfigurations databaseConfigurations)
    {
        if (databaseConfigurations.SqlServerEnabled)
        {
            optionsBuilder.UseSqlServer(databaseConfigurations.ConnectionString, options =>
            {
                options.MigrationsAssembly(CoreInfrastructurePersistenceSqlServerAssemblyMarker.Assembly.GetName().Name);
            });
        }
        else
        {
            optionsBuilder.UseNpgsql(databaseConfigurations.ConnectionString, options =>
            {
                options.MigrationsAssembly(CoreInfrastructurePersistencePostgresAssemblyMarker.Assembly.GetName().Name);
            });
        }
    }
}