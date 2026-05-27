using System.CommandLine;
using System.Reflection;
using GFSWeb.sdk;
using GFSWeb.sdk.Models;
using GFSWebTool.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Toolbox;
using Toolbox.Tools;

Console.WriteLine($"GFSWebTool CLI - Version {Assembly.GetExecutingAssembly().GetName().Version}");
Console.WriteLine();

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.SimpleConsole();
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<ICommand, ExportCommand>();
        services.AddSingleton<ICommand, ImportCommand>();
        services.AddSingleton<IAuthAccess, FakeAuthAccess>();
        services.AddCacheClient(x => x.ToLowerInvariant(), TimeSpan.FromMinutes(15));
        services.AddCacheClient<CommandRecord>(x => x.ToLowerInvariant(), TimeSpan.FromMinutes(15));
    })
    .Build();

var commands = host.Services.GetServices<ICommand>();
var rc = new RootCommand();
foreach (var item in commands) rc.Subcommands.Add(item.GetCommand());

var parserResult = rc.Parse(args);
return parserResult.Invoke();