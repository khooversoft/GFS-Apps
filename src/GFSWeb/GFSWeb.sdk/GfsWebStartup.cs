using System;
using System.Collections.Generic;
using System.Text;
using GFSWeb.sdk.Entity;
using Microsoft.Extensions.DependencyInjection;
using Toolbox;
using Toolbox.Tools;

namespace GFSWeb.sdk;

public static class GfsWebStartup
{
    public static IServiceCollection AddGFSWeb(this IServiceCollection services, Action<GfsWebOption> config)
    {
        config.NotNull();
        var option = new GfsWebOption();
        config(option);

        return services.AddGFSWeb(option);
    }

    public static IServiceCollection AddGFSWeb(this IServiceCollection services, GfsWebOption option)
    {
        option.NotNull();

        services.AddSqlClient<PrincipalIdentityStore>(c =>
        {
            c.ConnectionString = option.ConnectionString;
        });

        services.AddSingleton<GfsWebOption>(option);
        services.AddSingleton<PrincipalIdentityStore>();

        return services;
    }
}
