using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Toolbox.Data;
using Toolbox.Tools;

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
}
