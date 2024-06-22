using Microsoft.Extensions.Configuration;

using ProjectName.Infrastructure.Persistence.Common.Settings;

using Throw;

namespace ProjectName.Infrastructure.Persistence.Common.Configurations;

public static class ConfigurationsExtensions
{
    public static (DatabaseConfigurations ApplicationDbConfigurations, DatabaseConfigurations HangfireDbConfigurations)
        GetDbConfigurations(this IConfiguration configuration)
    {
        var applicationDatabaseConfigurations = configuration
            .GetSection(DatabaseConfigurations.ApplicationSectionName)
            .Get<DatabaseConfigurations>();

        applicationDatabaseConfigurations
            .ThrowIfNull(exceptionCustomizations: 
                DatabaseConfigurations.MissingSectionMessage(DatabaseConfigurations.ApplicationSectionName));
        
        var hangfireDbConfigurations = configuration
            .GetSection(DatabaseConfigurations.HangfireSectionName)
            .Get<DatabaseConfigurations>();
        
        hangfireDbConfigurations
            .ThrowIfNull(exceptionCustomizations: 
                DatabaseConfigurations.MissingSectionMessage(DatabaseConfigurations.HangfireSectionName));

        return (applicationDatabaseConfigurations, hangfireDbConfigurations);
    }
}