using GFSWeb.sdk.Models;
using GFSWeb.sdk.SqlParser;
using GFSWeb.sdk.Store;
using GFSWeb.sdk.Store.V2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Services;

public class ActivitySelect
{
    private readonly ILogger<ActivitySelect> _logger;
    private readonly ISqlClient<GFSAdminStore> _client;
    private readonly CommandEntity _commandEntity;

    public const string SqlType = "sql-command";
    public const string SapQueryType = "sap-query";
    public const string ExcelSourceType = "excel-source";

    public ActivitySelect(ISqlClient<GFSAdminStore> client, IServiceProvider serviceProvider, ILogger<ActivitySelect> logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();

        _commandEntity = ActivatorUtilities.CreateInstance<CommandEntity>(serviceProvider, client);
    }

    public async Task<IReadOnlyList<ActivityInfoRecord>> GetAll()
    {
        var commands = await GetCommands();

        var result = _builtIn
            .Concat(commands)
            .ToArray();

        return result;
    }

    public IPackageActivity CreateActivity(ActivityInfoRecord subject)
    {
        const string noOpSql = "SELECT 1;";
        subject.Validate().ThrowOnError();

        switch (subject.Type)
        {
            case SqlType:
                (string sqlCommand, string hash) = subject.Source switch
                {
                    CommandRecord command => (command.Data, command.Hash),
                    _ => (noOpSql, SqlParserTool.GetSqlCommandHash(noOpSql)),
                };

                return new SqlCommandActivity
                {
                    Id = subject.Id + "-" + RandomTool.RandomString(),
                    Description = subject.Description,
                    SqlCommand = sqlCommand,
                    Hash = hash,
                };

            case SapQueryType:
                return new SapQueryActivity
                {
                    Id = subject.Id + "-" + RandomTool.RandomString(),
                    Description = subject.Description,
                };

            case ExcelSourceType:
                return new ExcelReadSheetActivity
                {
                    Id = subject.Id + "-" + RandomTool.RandomString(),
                    Description = subject.Description,
                };

            default: throw new NotSupportedException();
        }
    }

    private async Task<IReadOnlyList<ActivityInfoRecord>> GetCommands()
    {
        var commands = await _commandEntity.GetAll();

        var result = commands
            .Select(x => new ActivityInfoRecord
            {
                Id = x.CommandId,
                Type = SqlType,
                Description = x.Description,
                Icon = "Icons.Material.Outlined.Rule",
                Source = x
            })
            .ToArray();

        return result;
    }

    private static readonly IReadOnlyList<ActivityInfoRecord> _builtIn = [
            new ActivityInfoRecord
            {
                Id = SapQueryType,
                Type = SapQueryType,
                Description = "Custom Query of SAP data",
                Icon = "Icons.Material.Outlined.Construction"
            },
            new ActivityInfoRecord
            {
                Id = ExcelSourceType,
                Type = ExcelSourceType,
                Description = "Excel spreadsheet is source",
                Icon = "Icons.Material.Outlined.Dataset"
            },
            new ActivityInfoRecord
            {
                Id = SqlType,
                Type = SqlType,
                Description = "SQL Command",
                Icon = "Icons.Material.Outlined.KeyboardCommandKey"
            },
        ];
}
