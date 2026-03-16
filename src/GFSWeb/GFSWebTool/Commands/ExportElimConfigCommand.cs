using System.CommandLine;
using GFSWeb.sdk.Models;
using GFSWebTool.Model;
using GFSWebTool.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        Option<FileInfo> configOption = new("--config")
        {
            Description = "Read connector configuration",
        };

        Option<DirectoryInfo> outputFolderOption = new("--output")
        {
            Description = "Folder to write Elim report package to",
        };

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
        var reportRecords = await ReadAndConstructReport(config, outputFolder);

        foreach (var reportRecord in reportRecords)
        {
            var conform = reportRecord.ShortName.NotEmpty().Select(x => char.IsLetterOrDigit(x) ? x : '_').ToArray();
            string fileName = new string(conform);
            var exportFileName = $"{fileName}.epack.json";

            var json = reportRecord.ToJson();
            string fullFileName = Path.Combine(outputFolder.FullName, exportFileName);

            _logger.LogInformation("Writing elim report package for ElimId={elimId}, shortName={shortName} to fleName={fileName}",
                reportRecord.Elimination.ID, reportRecord.ShortName, fullFileName);

            File.WriteAllText(fullFileName, json);
        }
    }

    private async Task<IReadOnlyList<ElimReportRecord>> ReadAndConstructReport(FileInfo config, DirectoryInfo outputFolder)
    {
        var store = CreateSqlStore(config);

        IReadOnlyList<EliminationRecord> eliminationList = (await store.GetEliminationRecords()).Select(x => x.ConvertTo()).ToArray();
        eliminationList.Count.Assert(x => x > 0, "No elimination records found in database");

        IReadOnlyList<ElimSelectRecord> elimSelectList = (await store.GetElimSelectRecords()).Select(x => x.ConvertTo()).ToArray();
        elimSelectList.Count.Assert(x => x > 0, "No selectList records found in database");

        IReadOnlyList<ElimSqlCommand> sqlCommandList = (await store.GetMiscTablesRecords()).Select(x => x.ConvertTo()).ToArray();
        sqlCommandList.Count.Assert(x => x > 0, "No miscTablesList records found in database");

        IReadOnlyList<ElimReportRecord> elimReportRecords = eliminationList
            .Select(elimination => new
            {
                ElimId = elimination.ID.NotNull(),
                Elimination = elimination,
            })
            .GroupJoin(
                elimSelectList,
                elimination => elimination.ElimId.ToString(),
                elimSelect => elimSelect.ElimID,
                (elimination, elimSelects) => new ElimReportRecord
                {
                    ShortName = elimination.Elimination.ShortName ?? string.Empty,
                    Description = elimination.Elimination.Def ?? string.Empty,
                    Elimination = elimination.Elimination,
                    ElimSelects = elimSelects.OrderBy(x => x.Pass).ThenBy(x => x.SubSeq).ToArray(),
                    SqlCommands = sqlCommandList.Where(x => x.CommandType?.Id == elimination.ElimId).ToArray(),
                }
            )
            .ToArray();

        var moreEliminations = eliminationList.Select(x => x.ID.ToString()).Except(elimSelectList.Select(x => x.ElimID)).Join(',');
        _logger.LogInformation("Elimination records with have no matching ElimSelect records: {moreEliminations}", moreEliminations);

        var moreSelects = elimSelectList.Select(x => x.ElimID).Except(eliminationList.Select(x => x.ID.ToString())).Join(',');
        _logger.LogInformation("ElimSelect records with have no matching Elimination records: {moreSelects}", moreSelects);

        _logger.LogInformation("Built {count} elim report records", elimReportRecords.Count);
        return elimReportRecords;
    }

    private EliminationImportStore CreateSqlStore(FileInfo connector)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(connector.FullName)
            .Build();

        var sqlWebToolOption = config.Get<GfsWebToolOption>().NotNull();
        var store = SqlClientTool.CreateSqlStore<EliminationImportStore>(sqlWebToolOption.OriginalConnectionString, _serviceProvider);
        //var sqlOption = new SqlOption { ConnectionString = sqlWebToolOption.OriginalConnectionString.NotEmpty() };
        //ISqlClient<EliminationImportStore> sqlClient = ActivatorUtilities.CreateInstance<SqlClient<EliminationImportStore>>(_serviceProvider, sqlOption);
        //sqlClient.TestConnection().BeTrue("Failed to connect to SQL database with provided configuration");

        //var store = ActivatorUtilities.CreateInstance<EliminationImportStore>(_serviceProvider, sqlClient);
        return store;
    }
}
