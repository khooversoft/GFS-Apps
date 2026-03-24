using System.CommandLine;
using GFSWeb.sdk.Models;
using GFSWeb.sdk.Store;
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
        Option<FileInfo> configOption = new("--config") { Description = "Read connector configuration" };
        Option<DirectoryInfo> inputFolderOption = new("--source") { Description = "Source folder to read Elim report package to update management DB" };

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

        var reportDirectory = await ReadReportDirectory(outputFolder);

        ReportPackageStore store = CreateSqlStore(config);

        await WriteMenu(store, reportDirectory);
        await WriteReportPackages(store, reportPackages, reportDirectory);
    }

    private async Task WriteMenu(ReportPackageStore store, ReportDirectoryModel reportDirectory)
    {
        foreach (var item in reportDirectory.Items)
        {
            (await store.Menu.Add(item)).BeOk();
            _logger.LogInformation("Import menu menuId={menuId}", item.MenuId);
        }
    }

    private async Task WriteReportPackages(ReportPackageStore store, IReadOnlyList<ReportPackageModel> reportPackages, ReportDirectoryModel reportDirectory)
    {
        foreach (ReportPackageModel reportPackage in reportPackages)
        {
            if (!reportDirectory.Items.Any(x => x.MenuId == reportPackage.MenuId))
            {
                _logger.LogInformation("Skipping packageId={packageId}, menuId={menuId} not found in ReportDirectory", reportPackage.PackageId, reportPackage.MenuId);
                continue;
            }

            var reportPackageRecord = new ReportPackageRecord
            {
                PackageId = reportPackage.PackageId,
                Description = reportPackage.Description,
                MenuId = reportPackage.MenuId,
                Data = reportPackage.ToJson(),
            };

            (await store.AddOrUpdate(reportPackageRecord)).BeOk();
            _logger.LogInformation("Imported Report package packageId={packageId}", reportPackage.PackageId);
        }
    }

    private async Task<IReadOnlyList<ReportPackageModel>> ReadReportPackages(DirectoryInfo inputFolder)
    {
        var reportRecords = new List<ReportPackageModel>();

        foreach (var file in inputFolder.GetFiles("*.reportPackage.json"))
        {
            string json = File.ReadAllText(file.FullName);
            var reportRecord = json.ToObject<ReportPackageModel>();
            reportRecords.Add(reportRecord);
        }

        return reportRecords;
    }

    private async Task<ReportDirectoryModel> ReadReportDirectory(DirectoryInfo inputFolder)
    {
        string dirFullFileName = Path.Combine(inputFolder.FullName, "ReportDirectory.json");
        string json = File.ReadAllText(dirFullFileName);

        var reportDirectory = json.ToObject<ReportDirectoryModel>().NotNull();
        return reportDirectory;
    }

    private ReportPackageStore CreateSqlStore(FileInfo connector)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(connector.FullName)
            .Build();

        var sqlWebToolOption = config.Get<GfsWebToolOption>().NotNull();
        var store = SqlClientTool.CreateSqlStore<ReportPackageStore>(sqlWebToolOption.ManagementConnectionString, _serviceProvider);
        return store;
    }
}
