using Core.Infrastructure.Persistence.Common.Services;

using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(EfCoreConfigurationsAssemblyProvider.GetEfCoreConfigurationsAssembly());
    }
}
