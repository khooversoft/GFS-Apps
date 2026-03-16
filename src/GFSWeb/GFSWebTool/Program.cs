using System.Reflection;
using Microsoft.Extensions.Hosting;
using Toolbox.Tools;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GFSWebTool.Commands;
using System.CommandLine;

Console.WriteLine($"GFSWebTool CLI - Version {Assembly.GetExecutingAssembly().GetName().Version}");
Console.WriteLine();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.SimpleConsole();
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<ICommand, ExportElimConfigCommand>();
        services.AddSingleton<ICommand, ImportElimConfigCommand>();
    })
    .Build();

var commands = host.Services.GetServices<ICommand>();
var rc = new RootCommand();
foreach (var item in commands) rc.Subcommands.Add(item.GetCommand());

var parserResult = rc.Parse(args);
return parserResult.Invoke();