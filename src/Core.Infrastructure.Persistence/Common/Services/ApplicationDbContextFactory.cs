using Core.Infrastructure.Persistence.Common.Extensions;
using Core.Infrastructure.Persistence.Common.Settings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Core.Infrastructure.Persistence.Common.Services;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .Build();

        var databaseConfigurations = new DatabaseConfigurations();
        
        configuration.GetSection(DatabaseConfigurations.SectionName).Bind(databaseConfigurations);
        
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        optionsBuilder.ConfigureFromDatabaseConfigurations(databaseConfigurations);
        
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}