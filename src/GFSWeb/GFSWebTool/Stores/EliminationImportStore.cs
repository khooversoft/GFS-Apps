using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWebTool.Stores;

internal class EliminationImportStore
{
    private readonly ISqlClient<EliminationImportStore> _client;
    private readonly ILogger<EliminationImportStore> _logger;

    public EliminationImportStore(ISqlClient<EliminationImportStore> client, ILogger<EliminationImportStore> logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<IReadOnlyList<EliminationImportRecord>> GetEliminationRecords()
    {
        var cmd = """
            SELECT  x.*
            FROM    [Eliminations] x
            ORDER BY x.[ShortName]
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .Execute<EliminationImportRecord>();

        return result;
    }

    public async Task<IReadOnlyList<ElimSelectImportRecord>> GetElimSelectRecords()
    {
        var cmd = """
            SELECT  x.*
            FROM    [ElimSelect] x
            ORDER BY x.[ElimID]
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .Execute<ElimSelectImportRecord>();

        return result;
    }

    public async Task<IReadOnlyList<MiscTablesImportRecord>> GetMiscTablesRecords()
    {
        var cmd = """
            SELECT  x.*
            FROM    [Misc_Tables] x
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .Execute<MiscTablesImportRecord>();

        return result;
    }
}
