using System.Collections.Frozen;
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

internal class ExportElimConfigCommand : ICommand
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ExportElimConfigCommand> _logger;
    private static readonly FrozenSet<string> _requiredTableIds = new[] { "UserList", "UserCoAccess", "UserAccess", "UserRoles" }.ToFrozenSet(StringComparer.OrdinalIgnoreCase);

    public ExportElimConfigCommand(IServiceProvider serviceProvider, ILogger<ExportElimConfigCommand> logger)
    {
        _serviceProvider = serviceProvider.NotNull();
        _logger = logger.NotNull();
    }

    public Command GetCommand()
    {
        Option<FileInfo> configOption = new("--config") { Description = "Read connector configuration" };
        Option<DirectoryInfo> outputFolderOption = new("--output") { Description = "Folder to write Elim report package to" };

        var command = new Command("export", "Export data from SQL database for Elim, ElimSelect, and SQL Commands");

        command.Options.Add(configOption);
        command.Options.Add(outputFolderOption);

        command.SetAction(async (parseResult, token) =>
        {
            FileInfo config = parseResult.GetValue(configOption).NotNull();
            DirectoryInfo outputFolder = parseResult.GetValue(outputFolderOption).NotNull();
            _logger.LogInformation("ConnectorFile={connectorFile}, outputFolder={outputFolder}", config.FullName, outputFolder.FullName);

            await Export(config, outputFolder);
        });

        return command;
    }

    private async Task Export(FileInfo config, DirectoryInfo outputFolder)
    {
        ClearPackages(outputFolder);
        var context = await ReadAllData(config);
        context = await ReadFxReconData(config, context);
        context = await BuildReportPackages(context);

        WriteDirectory(outputFolder, context);
        WriteReportPackages(outputFolder, context);
    }

    private void WriteDirectory(DirectoryInfo outputFolder, Context context)
    {
        var data = new ReportDirectoryModel
        {
            Items = context.ElimTreeList.Where(x => x.Parent == "top").Select(x => x.ConvertTo()).ToArray(),
            Users = context.UserList.Select(x => x.ConvertTo()).ToArray(),

            UserAccess = context.MiscTableList
                .Where(x => _requiredTableIds.Contains(x.Table_ID))
                .Select(x => x.ConvertTo())
                .ToArray(),

        };

        var dirJson = data.ToJson();

        string dirFullFileName = Path.Combine(outputFolder.FullName, "ReportDirectory.json");
        File.WriteAllText(dirFullFileName, dirJson);

        _logger.LogInformation("Writing elim report directory to fleName={fileName}", dirFullFileName);
    }

    private void WriteReportPackages(DirectoryInfo outputFolder, Context context)
    {
        foreach (var reportRecord in context.ReportPackages)
        {
            var conform = reportRecord.PackageId.Select(x => char.IsLetterOrDigit(x) ? x : '_').ToArray();
            string fileName = new string(conform);
            var exportFileName = $"{fileName}.reportPackage.json";

            var json = reportRecord.ToJson();
            string fullFileName = Path.Combine(outputFolder.FullName, exportFileName);

            _logger.LogInformation("Writing elim report package for PackageId={PackageId} to fleName={fileName}", reportRecord.PackageId, fullFileName);
            File.WriteAllText(fullFileName, json);
        }
    }

    private void ClearPackages(DirectoryInfo outputFolder)
    {
        var files = Directory.GetFiles(outputFolder.FullName, "*.reportPackage.json");
        _logger.LogInformation("Clear folder={folder} of report packages", outputFolder.FullName);
        foreach (var file in files)
        {
            File.Delete(file);
        }
    }
    private async Task<Context> BuildReportPackages(Context context)
    {
        _logger.LogInformation("Building report packages from elim tree records");

        var list = context.ElimTreeList
            .Where(x => x.Parent != "top")
            .Select(x => new ReportPackageModel
            {
                PackageId = x.Id.NotEmpty(),
                Description = x.Descr.NotEmpty(),
                MenuId = x.Parent,
                PackageType = ReportPackageModelTool.GetPackageType(x.PackageType),
                Elimination = lookupElimination(x.ShortName, x.Def),
                ElimSelects = lookupElimSelect(x.ElimId),
                MiscTables = getRequiredTables(x.Id),
            }).ToArray();

        context = context with { ReportPackages = list };
        return context;

        EliminationRecord? lookupElimination(string? shortName, string? def) => (shortName, def) switch
        {
            (string s, string d) => context.EliminationList.FirstOrDefault(x => x.ShortName == s && x.Def == d),
            _ => null,
        };

        IReadOnlyList<ElimSelectRecord> lookupElimSelect(int? elimId) => elimId switch
        {
            int id => id.ToString().Func(x => context.ElimSelectList.Where(y => y.ElimID == x).ToArray()),
            _ => Array.Empty<ElimSelectRecord>(),
        };

        IReadOnlyList<MiscTablesRecord> getRequiredTables(string id) => context.MiscTableList
            .Where(x => TableIdMap.IsRequiredTableId(id, x))
            .OrderBy(x => x.Table_ID)
            .ThenBy(x => x.ID)
            .ToArray();
    }

    private CorpAccountStore CreateCorpAccountStore(FileInfo connector)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(connector.FullName)
            .Build();

        var sqlWebToolOption = config.Get<GfsWebToolOption>().NotNull();
        _logger.LogInformation("Creating SQL store with connectionString={connectionString}", sqlWebToolOption.CorpAccountConnection);
        var store = SqlClientTool.CreateSqlStore<CorpAccountStore>(sqlWebToolOption.CorpAccountConnection, _serviceProvider);
        return store;
    }

    private CorpAccountStore CreateFxReconStore(FileInfo connector)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(connector.FullName)
            .Build();

        var sqlWebToolOption = config.Get<GfsWebToolOption>().NotNull();
        _logger.LogInformation("Creating SQL store with connectionString={connectionString}", sqlWebToolOption.FxReconConnection);
        var store = SqlClientTool.CreateSqlStore<CorpAccountStore>(sqlWebToolOption.FxReconConnection, _serviceProvider);
        return store;
    }

    private async Task<Context> ReadAllData(FileInfo config)
    {
        var store = CreateCorpAccountStore(config);

        _logger.LogInformation("Reading elimination records from database");
        IReadOnlyList<EliminationRecord> eliminationList = await store.GetEliminationRecords();
        eliminationList.Count.Assert(x => x > 0, "No elimination records found in database");

        _logger.LogInformation("Reading elim select records from database");
        IReadOnlyList<ElimSelectRecord> elimSelectList = await store.GetElimSelectRecords();
        elimSelectList.Count.Assert(x => x > 0, "No selectList records found in database");

        _logger.LogInformation("Reading misc table records from database");
        IReadOnlyList<MiscTablesRecord> miscTableList = await store.GetMiscTablesRecords();
        miscTableList.Count.Assert(x => x > 0, "No miscTableList records found in database");

        _logger.LogInformation("Reading elim tree records from database");
        IReadOnlyList<ElimTreeRecord> elimTreeRecords = await store.GetMenuTree();
        elimTreeRecords.Count.Assert(x => x > 0, "No menu tree records found in database");

        IReadOnlyList<UserRecord> userRecords = await store.GetUserRecords();
        userRecords.Where(x => x.Email.IsEmpty()).ForEach(x => _logger.LogError("Skipping user record with empty email UserID={UserID}", x.UserID_Network));
        userRecords.Count.Assert(x => x > 0, "No user records found in database");
        userRecords = userRecords.Where(x => x.Email.IsNotEmpty()).ToArray();

        return new Context
        {
            EliminationList = eliminationList,
            ElimSelectList = elimSelectList,
            MiscTableList = miscTableList,
            ElimTreeList = elimTreeRecords,
            UserList = userRecords,
        };
    }

    private async Task<Context> ReadFxReconData(FileInfo config, Context context)
    {
        var store = CreateFxReconStore(config);

        _logger.LogInformation("Reading Misc_Tables for user access from FxRecon database");
        IReadOnlyList<MiscTablesRecord> miscTableList = await store.GetMiscTablesRecords();
        miscTableList.Count.Assert(x => x > 0, "No miscTableList records found in database");

        var addMiscTables = miscTableList.Where(x => _requiredTableIds.Contains(x.Table_ID)).ToArray();

        context = context with
        {
            MiscTableList = context.MiscTableList
                .Where(x => !_requiredTableIds.Contains(x.Table_ID))
                .Concat(addMiscTables)
                .ToArray(),
        };

        return context;
    }

    private record Context
    {
        public IReadOnlyList<EliminationRecord> EliminationList { get; init; } = null!;
        public IReadOnlyList<ElimSelectRecord> ElimSelectList { get; init; } = null!;
        public IReadOnlyList<MiscTablesRecord> MiscTableList { get; init; } = null!;
        public IReadOnlyList<ElimTreeRecord> ElimTreeList { get; init; } = null!;
        public IReadOnlyList<UserRecord> UserList { get; init; } = null!;
        public IReadOnlyList<ReportPackageModel> ReportPackages { get; init; } = null!;
    }
}
