using Microsoft.Extensions.DependencyInjection;

namespace Toolbox.SAP.sdk;

public static class ServiceExtensions
{
    public static IServiceCollection AddSapService(this IServiceCollection services, SapOption option)
    {
        ArgumentNullException.ThrowIfNull(option);
        services.AddSingleton(option);
        services.AddSingleton<ISapService, SapService>();
        return services;
    }
}
