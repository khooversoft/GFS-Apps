using GFSWeb.sdk.Store;
using Microsoft.Extensions.DependencyInjection;
using Toolbox;
using Toolbox.Tools;

namespace GFSWeb.sdk;

public static class GfsWebStartup
{
    //public static IServiceCollection AddGFSWeb(this IServiceCollection services, Action<GfsWebOption> config)
    //{
    //    config.NotNull();
    //    var option = new GfsWebOption();
    //    config(option);

    //    return services.AddGFSWeb(option);
    //}

    public static IServiceCollection AddGFSWeb(this IServiceCollection services, GfsWebOption webOption, GfsSapOption sapOption)
    {
        webOption.NotNull();
        sapOption.NotNull();

        services.AddSqlClient<PrincipalIdentityStore>(c =>
        {
            c.ConnectionString = webOption.ConnectionString;
        });

        services.AddSingleton<GfsWebOption>(webOption);
        services.AddSingleton<GfsSapOption>(sapOption);
        services.AddSingleton<PrincipalIdentityStore>();

        return services;
    }
}
