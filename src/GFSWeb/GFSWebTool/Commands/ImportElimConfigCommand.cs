using System.CommandLine;
using GFSWeb.sdk.Models;
using GFSWeb.sdk.Store;
using GFSWeb.sdk.Store.V2;
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

        GFSAdminStore store = CreateAdminStore(config);

        await WriteMenu(store, reportDirectory);
        await WritePrincipalIdentities(store, reportDirectory);
        await WriteUserAccess(store, reportDirectory);
        await WriteReportPackages(store, reportPackages, reportDirectory);

        _logger.LogInformation("Running import fixup");
        await store.Package.ImportFixup();
    }

    private async Task WriteMenu(GFSAdminStore store, ReportDirectoryModel reportDirectory)
    {
        _logger.LogInformation("Importing menu from ReportDirectory, total={total}", reportDirectory.Items.Count);
        foreach (var item in reportDirectory.Items)
        {
            (await store.Menu.Add(item)).BeOk();
            _logger.LogInformation("Import menu menuId={menuId}", item.MenuId);
        }
    }

    public async Task WritePrincipalIdentities(GFSAdminStore store, ReportDirectoryModel reportDirectory)
    {
        _logger.LogInformation("Importing users from ReportDirectory, total={total}", reportDirectory.Items.Count);

        foreach (var item in reportDirectory.Users)
        {
            (await store.Identity.AddOrUpdate(item)).BeOk();
            _logger.LogInformation("Import user nameIdentifier={menuId}", item.NameIdentifier);
        }
    }

    public async Task WriteUserAccess(GFSAdminStore store, ReportDirectoryModel reportDirectory)
    {
        _logger.LogInformation("Importing userAccess from ReportDirectory, total={total}", reportDirectory.Items.Count);

        IEnumerable<UserAccessRecord> addUserAccess = reportDirectory.UserAccess.Where(x => x.IsUserCoAccess()).ToArray();

        foreach (var item in addUserAccess)
        {
            Toolbox.Types.Option<int> result = await store.UserAccess.AddOrUpdate(item);
            if (result.StatusCode == Toolbox.Types.StatusCode.BadRequest)
            {
                _logger.LogWarning("Skipping import user UserAccess={userAccess}, principal does not exist", item.ToString());
                continue;
            }

            _logger.LogInformation("Import user UserAccess={userAccess}", item.ToString());
        }
    }

    private async Task WriteReportPackages(GFSAdminStore store, IReadOnlyList<ReportPackageModel> reportPackages, ReportDirectoryModel reportDirectory)
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

            (await store.Package.AddOrUpdate(reportPackageRecord)).BeOk();
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

    private GFSAdminStore CreateAdminStore(FileInfo connector)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(connector.FullName)
            .Build();

        var sqlWebToolOption = config.Get<GfsWebToolOption>().NotNull();
        var store = SqlClientTool.CreateSqlStore<GFSAdminStore>(sqlWebToolOption.GfsWebConnection, _serviceProvider);
        return store;
    }
}
