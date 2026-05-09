using System.Data;
using GFSWeb.sdk.Models;
using GFSWeb.sdk.SqlParser;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Extensions;
using Toolbox.Tools;

namespace GFSWeb.sdk.Store.V1;

public class CorpAccountStore
{
    private readonly ISqlClient<CorpAccountStore> _client;
    private readonly ILogger<CorpAccountStore> _logger;

    public CorpAccountStore(ISqlClient<CorpAccountStore> client, ILogger<CorpAccountStore> logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<IReadOnlyList<EliminationRecord>> GetEliminationRecords()
    {
        var cmd = """
            SELECT  x.*
            FROM    [Eliminations] x
            ORDER BY x.[ShortName]
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .Execute<EliminationRecord>();

        return result;
    }

    public async Task<IReadOnlyList<ElimSelectRecord>> GetElimSelectRecords()
    {
        var cmd = """
            SELECT  x.*
            FROM    [ElimSelect] x
            ORDER BY x.[ElimID]
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .Execute<ElimSelectRecord>();

        return result;
    }

    public async Task<IReadOnlyList<MiscTablesRecord>> GetMiscTablesRecords()
    {
        var cmd = """
            SELECT  x.*
            FROM    [Misc_Tables] x
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .Execute<MiscTablesRecord>();

        return result;
    }

    public async Task<IReadOnlyList<UserRecord>> GetUserRecords()
    {
        var cmd = """
            SELECT  x.*
            FROM    [Users] x
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .Execute<UserRecord>();

        return result;
    }

    public async Task<IReadOnlyList<ElimTreeRecord>> GetMenuTree()
    {
        var cmd = AssemblyResource.GetResourceString("GFSWeb.sdk.Scripts.MenuTreeSelect.sql", typeof(ElimTreeRecord));

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .Execute<ElimTreeRecord>();

        var updated = result.Select(x => x.ParseDetails()).ToArray();
        return updated;
    }

    //public async Task<IReadOnlyList<CommandRecord>> GetCommonCommands()
    //{
    //    var cmd = """
    //        SELECT  x.descr, Cnt=count(*)
    //        FROM    [Misc_Tables] x
    //        WHERE   x.Table_Id like 'SQL-%'
    //        GROUP BY x.Descr HAVING count(*) >= 10
    //        ORDER BY COUNT(*) DESC
    //        """;

    //    var result = await _client.Query()
    //        .SetCommand(cmd, CommandType.Text)
    //        .Execute<ReadGroupRecord>();

    //    var list = new List<CommandRecord>();
    //    foreach(var item in result)
    //    {
    //        var parseResult = SqlParserTool.FormatLine(item.Descr);
    //        if (parseResult.Errors.Count > 0)
    //        {
    //            _logger.LogError("SQL parse errors for command '{Command}': {Errors}", item.Descr, parseResult.Errors);
    //            continue;
    //        }

    //        string formattedSql = parseResult.formattedSql;
    //        string hash = formattedSql.ToHashHex();

    //        var commandRecord = new CommandRecord
    //        {
    //            CommandId = hash,
    //            Description = "xxx",
    //            Data = formattedSql,
    //            Disabled = false
    //        };
    //    }

    //    return list;
    //}

    //private record ReadGroupRecord
    //{
    //    public string Descr { get; set; } = null!;
    //}
}
