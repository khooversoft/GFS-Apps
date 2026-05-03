using GFSWeb.sdk.Store;
using GFSWeb.sdk.Store.V2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Toolbox;
using Toolbox.Data;
using Toolbox.Tools;

namespace GFSWeb.sdk;

public static class GfsWebStartup
{
    public static IServiceCollection AddGFSWeb(this IServiceCollection services, GfsWebOption webOption, GfsSapOption sapOption)
    {
        webOption.NotNull();
        sapOption.NotNull();

        services.AddSqlClient<GFSAdminStore>(c =>
        {
            c.ConnectionString = webOption.AdminConnectionString;
        });

        services.AddSingleton<GfsWebOption>(webOption);
        services.AddSingleton<GfsSapOption>(sapOption);
        services.AddScoped<GFSAdminStore>();

        services.AddTransient<UserDatalakeStore>(service =>
        {
            var datalakeOption = new DatalakeOption
            {
                Account = webOption.UserStore.Account,
                Container = webOption.UserStore.Container,
                BasePath = webOption.UserStore.BasePath,
                Credentials = webOption.Credentials
            };

            return ActivatorUtilities.CreateInstance<UserDatalakeStore>(service, datalakeOption);
        });

        services.AddTransient<ScheduleDatalakeStore>(service =>
        {
            var datalakeOption = new DatalakeOption
            {
                Account = webOption.ScheduleStore.Account,
                Container = webOption.ScheduleStore.Container,
                BasePath = webOption.ScheduleStore.BasePath,
                Credentials = webOption.Credentials
            };

            return ActivatorUtilities.CreateInstance<ScheduleDatalakeStore>(service, datalakeOption);
        });

        return services;
    }
}

public class ScheduleDatalakeStore : DatalakeStore
{
    public ScheduleDatalakeStore(DatalakeOption datalakeOption, ILogger<DatalakeStore> logger) : base(datalakeOption, logger)
    {
    }
}