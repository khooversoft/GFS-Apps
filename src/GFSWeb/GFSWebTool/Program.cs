using System.CommandLine;
using System.Reflection;
using GFSWebTool.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Toolbox.Tools;

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