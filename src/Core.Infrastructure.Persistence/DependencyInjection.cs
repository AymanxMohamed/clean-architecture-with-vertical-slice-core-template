using System.Reflection;

using Core.Application.Common.Persistence;
using Core.Application.Common.Users;
using Core.Infrastructure.Persistence.Common.Extensions;
using Core.Infrastructure.Persistence.Common.Repositories;
using Core.Infrastructure.Persistence.Common.Services;
using Core.Infrastructure.Persistence.Common.Settings;
using Core.Infrastructure.Persistence.Users.Repositories;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreInfrastructurePersistence(
        this IServiceCollection services, 
        IConfiguration configuration,
        Assembly efCoreConfigurationsAssembly)
    {
        return services
            .AddDbContextFromDbConfigurations(configuration, efCoreConfigurationsAssembly)
            .AddGenericRepository()
            .AddUsers();
    }
    
    private static IServiceCollection AddDbContextFromDbConfigurations(
        this IServiceCollection services, 
        IConfiguration configuration,
        Assembly efCoreConfigurationsAssembly)
    {
        EfCoreConfigurationsAssemblyProvider.SetEfCoreConfigurationsAssembly(efCoreConfigurationsAssembly);
        
        var databaseConfigurations = new DatabaseConfigurations();
        
        configuration.GetSection(DatabaseConfigurations.SectionName).Bind(databaseConfigurations);

        services.AddDbContext<ApplicationDbContext>(builder =>
        {
            builder.ConfigureFromDatabaseConfigurations(databaseConfigurations);
        });

        return services;
    }

    private static IServiceCollection AddGenericRepository(this IServiceCollection services)
    {
        services.AddTransient(
            serviceType: typeof(ISpecificationEvaluator<,>),  
            implementationType: typeof(SpecificationEvaluator<,>));
        
        services.AddTransient(
            serviceType: typeof(IGenericRepository<,>),  
            implementationType: typeof(GenericRepository<,>));

        services.AddTransient(
            serviceType: typeof(ISpecification<,>),  
            implementationType: typeof(SpecificationBase<,>));
        
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        
        return services;
    }

    private static IServiceCollection AddUsers(this IServiceCollection services)
    {
        services.AddTransient<IUsersCommandRepository, UsersCommandRepository>();
        services.AddTransient<IUsersQueryRepository, UsersQueryRepository>();
        return services;
    }
}