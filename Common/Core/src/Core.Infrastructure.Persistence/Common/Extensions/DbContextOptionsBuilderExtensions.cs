using Core.Infrastructure.Persistence.Common.Settings;

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
                options.MigrationsAssembly(CoreInfrastructurePersistenceAssemblyMarker.Assembly.GetName().Name);
                options.MigrationsHistoryTable("__EFMigrationsHistory", "sqlserver");
            });
        }
        else
        {
            optionsBuilder.UseNpgsql(databaseConfigurations.ConnectionString, options =>
            {
                options.MigrationsAssembly(CoreInfrastructurePersistenceAssemblyMarker.Assembly.GetName().Name);
                options.MigrationsHistoryTable("__EFMigrationsHistory", "postgres");
            });
        }
    }
}