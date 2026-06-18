using GFSWeb.sdk.Models;
using GFSWeb.sdk.Services;
using GFSWeb.sdk.Store;
using GFSWeb.sdk.Store.Azure;
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

        services.AddMemoryCache();

        services.AddSqlClient<GFSAdminStore>(c =>
        {
            c.ConnectionString = webOption.AdminConnectionString;
        });

        services.AddSingleton<GfsWebOption>(webOption);
        services.AddScoped<ActivitySelect>();
        services.AddScoped<GFSAdminStore>();
        services.AddCacheClient(x => x.ToLowerInvariant(), TimeSpan.FromMinutes(15));
        services.AddCacheClient<CommandRecord>(x => x.ToLowerInvariant(), TimeSpan.FromMinutes(15));

        services.AddTransient<UserFileStore>(service =>
        {
            var datalakeOption = webOption.ConvertTo(() => webOption.UserStore);
            return ActivatorUtilities.CreateInstance<UserFileStore>(service, datalakeOption);
        });

        services.AddTransient<ScheduleFileStore>(service =>
        {
            var datalakeOption = webOption.ConvertTo(() => webOption.ScheduleStore);
            return ActivatorUtilities.CreateInstance<ScheduleFileStore>(service, datalakeOption);
        });

        services.AddTransient<SharedFileStore>(service =>
        {
            var datalakeOption = webOption.ConvertTo(() => webOption.GeneralStore);
            return ActivatorUtilities.CreateInstance<SharedFileStore>(service, datalakeOption);
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