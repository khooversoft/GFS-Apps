using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;
using GFSWeb.sdk.Entity;
using GFSWeb.sdk.Models;
using GFSWebTool.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Extensions;
using Toolbox.Tools;

namespace GFSWebTool.Commands;

internal class ImportElimConfigCommand : ICommand
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ImportElimConfigCommand> _logger;

    public ImportElimConfigCommand(IServiceProvider serviceProvider, ILogger<ImportElimConfigCommand> logger)
    {
        _serviceProvider = serviceProvider.NotNull();
        _logger = logger.NotNull();
    }

    public Command GetCommand()
    {
        Option<FileInfo> configOption = new("--config")
        {
            Description = "Read connector configuration",
        };

        Option<DirectoryInfo> inputFolderOption = new("--source")
        {
            Description = "Source folder to read Elim report package to update management DB",
        };

        var command = new Command("import", "Import Elim report packages to management DB");
        command.Options.Add(configOption);
        command.Options.Add(inputFolderOption);

        command.SetAction(async (parseResult, token) =>
        {
            FileInfo config = parseResult.GetValue(configOption).NotNull();
            DirectoryInfo inputFolder = parseResult.GetValue(inputFolderOption).NotNull();
            _logger.LogInformation("ConnectorFile={connectorFile}, source={source}", config.FullName, inputFolder.FullName);
            await Import(config, inputFolder);
        });

        return command;
    }

    private async Task Import(FileInfo config, DirectoryInfo outputFolder)
    {
        var reportPackages = await ReadReportPackages(outputFolder);
        reportPackages.Count.Assert(x => x > 0, "No report package found in input folder");

        ElimOperationStore store = CreateSqlStore(config);

        foreach (ElimReportRecord reportPackage in reportPackages)
        {
            var ElimOperationRecord = new ElimOperationRecord
            {
                ElimCode = reportPackage.ShortName,
                Description = reportPackage.Description,
                Data = reportPackage.ToJson(),
            };

            (await store.AddUpdateElimOperation(ElimOperationRecord)).BeOk();
            _logger.LogInformation("Report package {shortName} imported to management DB", reportPackage.ShortName);
        }
    }

    private async Task<IReadOnlyList<ElimReportRecord>> ReadReportPackages(DirectoryInfo inputFolder)
    {
        var reportRecords = new List<ElimReportRecord>();

        foreach (var file in inputFolder.GetFiles("*.epack.json"))
        {
            string json = File.ReadAllText(file.FullName);
            var reportRecord = json.ToObject<ElimReportRecord>();
            reportRecords.Add(reportRecord);
        }

        return reportRecords;
    }

    private ElimOperationStore CreateSqlStore(FileInfo connector)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(connector.FullName)
            .Build();

        var sqlWebToolOption = config.Get<GfsWebToolOption>().NotNull();
        var store = SqlClientTool.CreateSqlStore<ElimOperationStore>(sqlWebToolOption.ManagementConnectionString, _serviceProvider);
        return store;
    }
}
