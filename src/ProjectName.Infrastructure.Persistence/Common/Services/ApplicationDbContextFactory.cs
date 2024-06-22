using ProjectName.Infrastructure.Persistence.Common.Extensions;
using ProjectName.Infrastructure.Persistence.Common.Settings;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProjectName.Infrastructure.Persistence.Common.Services;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
            .Build();

        var databaseConfigurations = new DatabaseConfigurations();
        
        configuration.GetSection(DatabaseConfigurations.ApplicationSectionName).Bind(databaseConfigurations);
        
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        optionsBuilder.ConfigureFromDatabaseConfigurations(databaseConfigurations);
        
        return new ApplicationDbContext(optionsBuilder.Options, new HttpContextAccessor());
    }
}