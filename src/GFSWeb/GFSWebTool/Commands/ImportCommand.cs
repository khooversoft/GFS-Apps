using System.CommandLine;
using GFSWeb.sdk.Models;
using GFSWeb.sdk.SqlParser;
using GFSWeb.sdk.Store;
using GFSWeb.sdk.Store.V2;
using GFSWebTool.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Extensions;
using Toolbox.Tools;

namespace GFSWebTool.Commands;

internal class ImportCommand : ICommand
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ImportCommand> _logger;

    public ImportCommand(IServiceProvider serviceProvider, ILogger<ImportCommand> logger)
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

    private async Task Import(FileInfo config, DirectoryInfo inputFolder)
    {
        var reportPackages = await ReadReportPackages(inputFolder);
        reportPackages.Count.Assert(x => x > 0, "No report package found in input folder");

        var reportDirectory = await ReadReportDirectory(inputFolder);

        GFSAdminStore store = CreateAdminStore(config);

        await WriteMenu(store, reportDirectory);
        await WritePrincipalIdentities(store, reportDirectory);
        await WriteUserAccess(store, reportDirectory);
        await WriteReportPackages(store, reportPackages, reportDirectory);
        await WriteCommonCommands(store, inputFolder, reportPackages);

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

    private async Task WriteCommonCommands(GFSAdminStore store, DirectoryInfo inputFolder, IReadOnlyList<ReportPackageModel> reportPackages)
    {
        CommandRecordPackage? commandRecordPackage = null;

        string currentCommonCommands = Path.Combine(inputFolder.FullName, @"Settings\CommonCommands.json");
        if (File.Exists(currentCommonCommands))
        {
            string json = File.ReadAllText(currentCommonCommands);

            commandRecordPackage = json.ToObject<CommandRecordPackage>().NotNull();
            commandRecordPackage.Validate().BeOk("Invalid common command package");
        }

        var commonCommands = new List<string>();

        foreach (MiscTablesRecord item in reportPackages.SelectMany(x => x.MiscTables))
        {
            if (!item.Table_ID.StartsWith("SQL-", StringComparison.OrdinalIgnoreCase)) continue;

            var parserResult = SqlParserTool.FormatLine(item.Descr);
            if (parserResult.Errors.Count > 0)
            {
                _logger.LogError("SQL parse errors for command '{Command}'", item.Descr);
                continue;
            }

            commonCommands.Add(parserResult.formattedSql.ToLowerInvariant());
        }

        // Get top repeating commands
        var topCommands = commonCommands
            .GroupBy(x => x)
            .Where(x => x.Count() > 10)
            .Select(x => SqlParserTool.GenerateCommand(x.First()))
            .OfType<CommandRecord>() 
            .ToArray();

        var dict = topCommands.ToDictionary(x => x.CommandId, x => x);
        commandRecordPackage?.Commands.ForEach(x => dict[x.CommandId] = x);

        foreach (var commandRecord in dict.Values)
        {
            var result = await store.Command.Upsert(commandRecord);
            if (result.StatusCode != Toolbox.Types.StatusCode.OK)
            {
                _logger.LogError("Failed to upsert common command CommandId={commandId}, error={error}", commandRecord.CommandId, result.Error);
                continue;
            }

            _logger.LogInformation("Import command CommandId={commandId}", commandRecord.CommandId);
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
