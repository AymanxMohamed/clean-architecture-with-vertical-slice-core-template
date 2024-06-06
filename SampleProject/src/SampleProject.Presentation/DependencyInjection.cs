using Microsoft.Extensions.DependencyInjection;

namespace SampleProject.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        return services;
    }
}