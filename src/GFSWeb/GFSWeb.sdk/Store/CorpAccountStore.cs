using System.Data;
using GFSWeb.sdk.Models;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;

namespace GFSWeb.sdk.Store;

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
}
