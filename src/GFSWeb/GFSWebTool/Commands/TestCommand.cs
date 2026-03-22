using System.CommandLine;
using Microsoft.Extensions.Logging;
using Toolbox.Tools;

namespace GFSWebTool.Commands;

internal class TestCommand : ICommand
{
    private readonly ILogger<TestCommand> _logger;

    public TestCommand(ILogger<TestCommand> logger) => _logger = logger;

    public Command GetCommand()
    {
        Option<FileInfo> fileOption = new("--file")
        {
            Description = "The file to read and display on the console"
        };

        var command = new Command("name", "description");
        command.Options.Add(fileOption);

        command.SetAction(parseResult =>
        {
            FileInfo file = parseResult.GetValue(fileOption).NotNull();
            Console.WriteLine("File: " + file.FullName);
            _logger.LogInformation("File: {file}", file.FullName);
        });

        return command;
    }
}
