using ProjectName.Infrastructure.Persistence.Common.Settings;

using Microsoft.EntityFrameworkCore;

namespace ProjectName.Infrastructure.Persistence.Common.Extensions;

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
                options.MigrationsAssembly(databaseConfigurations.MigrationAssembly);
            });
        }
        else
        {
            optionsBuilder.UseNpgsql(databaseConfigurations.ConnectionString, options =>
            {
                options.MigrationsAssembly(databaseConfigurations.MigrationAssembly);
            });
        }
    }
}