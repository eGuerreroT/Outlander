using Microsoft.Extensions.DependencyInjection;

namespace Outlander.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOutlander(
        this IServiceCollection services)
    {
        // Servicios futuros

        // services.AddScoped<IOutlanderDialogService, OutlanderDialogService>();
        // services.AddScoped<IOutlanderToastService, OutlanderToastService>();

        return services;
    }
}