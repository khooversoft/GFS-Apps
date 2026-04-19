using Microsoft.Extensions.DependencyInjection;
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

        services.AddSingleton<ISqlClient<T>>(services =>
        {
            return ActivatorUtilities.CreateInstance<SqlClient<T>>(services, option);
        });

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
}
