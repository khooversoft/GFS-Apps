using System.CommandLine;
using GFSWeb.sdk.Models;
using GFSWebTool.Model;
using GFSWeb.sdk.Store;
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
        context = await BuildReportPackages(context);

        var dirJson = new ReportDirectoryModel { Items = context.ElimTreeRecords }.ToJson();

        string dirFullFileName = Path.Combine(outputFolder.FullName, "ReportDirectory.json");
        File.WriteAllText(dirFullFileName, dirJson);
        _logger.LogInformation("Writing elim report directory to fleName={fileName}", dirFullFileName);

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

        var list = context.ElimTreeRecords
            .Where(x => x.Parent != "top")
            .Select(x => new ReportPackageModel
            {
                PackageId = x.Id.NotEmpty(),
                SortKey = x.SortKey.NotEmpty(),
                Description = x.Descr.NotEmpty(),
                ParentPackageId = x.Parent.NotEmpty(),
                PackageType = ReportPackageModelTool.GetPackageType(x.PackageType),
                Elimination = lookupElimination(x.ShortName, x.Def),
                ElimSelects = lookupElimSelect(x.ElimId),
                SqlCommand = lookupSqlCommand(x.Id),
                SqlSelect = lookupSqlSelect(x.Id),
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

        IReadOnlyList<MiscTablesRecord> lookupSqlCommand(string id) => context.MiscTableList
            .Where(x => x.Table_ID == "SQL-" + id)
            .OrderBy(x => x.ID)
            .ToArray();

        IReadOnlyList<MiscTablesRecord> lookupSqlSelect(string id) => context.MiscTableList
            .Where(x => x.Table_ID.EqualsIgnoreCase("SQLselect") && x.ID.Like(id + "*"))
            .OrderBy(x => x.ID)
            .ToArray();
    }

    private EliminationImportStore CreateSqlStore(FileInfo connector)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(connector.FullName)
            .Build();

        var sqlWebToolOption = config.Get<GfsWebToolOption>().NotNull();
        _logger.LogInformation("Creating SQL store with connectionString={connectionString}", sqlWebToolOption.OriginalConnectionString);
        var store = SqlClientTool.CreateSqlStore<EliminationImportStore>(sqlWebToolOption.OriginalConnectionString, _serviceProvider);
        return store;
    }

    private async Task<Context> ReadAllData(FileInfo config)
    {
        var store = CreateSqlStore(config);

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

        return new Context
        {
            EliminationList = eliminationList,
            ElimSelectList = elimSelectList,
            MiscTableList = miscTableList,
            ElimTreeRecords = elimTreeRecords,
        };
    }

    private record Context
    {
        public IReadOnlyList<EliminationRecord> EliminationList { get; init; } = null!;
        public IReadOnlyList<ElimSelectRecord> ElimSelectList { get; init; } = null!;
        public IReadOnlyList<MiscTablesRecord> MiscTableList { get; init; } = null!;
        public IReadOnlyList<ElimTreeRecord> ElimTreeRecords { get; init; } = null!;
        public IReadOnlyList<ReportPackageModel> ReportPackages { get; init; } = null!;
    }
}
