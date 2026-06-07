using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace Toolbox;

public static class Startup
{
    public static IServiceCollection AddSqlClient<T>(this IServiceCollection services, Action<SqlOption> config)
    {
        config.NotNull();

        var option = new SqlOption();
        config(option);

        services.AddSingleton<ISqlClient<T>>(services => ActivatorUtilities.CreateInstance<SqlClient<T>>(services, option));
        return services;
    }

    public static IServiceCollection AddDatalakeFileStore(this IServiceCollection services, DatalakeOption datalakeOption)
    {
        datalakeOption.NotNull();
        datalakeOption.Validate().ThrowOnError("Invalid DatalakeOption");

        services.AddSingleton(datalakeOption);
        services.AddSingleton<DatalakeStore>();

        return services;
    }

    public static IServiceCollection AddCacheClient(this IServiceCollection services, Func<string, string> getKey, TimeSpan cacheTime)
    {
        services.NotNull();
        getKey.NotNull();

        services.AddMemoryCache();
        services.TryAddSingleton<ICacheClient>(services => ActivatorUtilities.CreateInstance<CacheClient>(services, getKey, cacheTime));
        return services;
    }

    public static IServiceCollection AddCacheClient<T>(this IServiceCollection services, Func<string, string> getKey, TimeSpan cacheTime)
    {
        services.NotNull();
        getKey.NotNull();

        services.AddMemoryCache();
        services.TryAddSingleton<ICacheClient<T>>(services => ActivatorUtilities.CreateInstance<CacheClient<T>>(services, getKey, cacheTime));
        return services;
    }
}
