using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Entity;

public class ElimOperationStore
{
    private readonly ISqlClient<ElimOperationStore> _client;
    private readonly ILogger<ElimOperationStore> _logger;

    public ElimOperationStore(ISqlClient<ElimOperationStore> client, ILogger<ElimOperationStore> logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<Option<ElimOperationRecord>> Get(string elimCode)
    {
        var cmd = """
            SELECT  x.*
            FROM    [App].[ElimOperation] x
            WHERE   x.[ElimCode] = @elimCode
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .AddParameter("@ElimCode", elimCode)
            .Execute<ElimOperationRecord>();

        return result.Count switch
        {
            0 => StatusCode.NotFound,
            1 => result[0],
            _ => throw new InvalidOperationException($"Multiple records found for NameIdentifier: {elimCode}")
        };
    }

    public async Task<IReadOnlyList<ElimOperationRecord>> GetAll()
    {
        var cmd = """
            SELECT  x.*
            FROM    [App].[ElimOperation] x
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .Execute<ElimOperationRecord>();

        return result;
    }

    public async Task<Option<int>> Add(ElimOperationRecord record)
    {
        record.NotNull().Validate().ThrowOnError();

        var result = await _client.Query()
            .SetCommand("[App].[AddElimOperation]", CommandType.StoredProcedure)
            .AddParameter("@ElimCode", record.ElimCode)
            .AddParameter("@Description", record.Description)
            .AddParameter("@Data", record.Data)
            .ExecuteNonQuery();

        return result;
    }

    public async Task<Option<int>> AddUpdateElimOperation(ElimOperationRecord record)
    {
        record.NotNull().Validate().ThrowOnError();

        var result = await _client.Query()
            .SetCommand("[App].[AddUpdateElimOperation]", CommandType.StoredProcedure)
            .AddParameter("@ElimCode", record.ElimCode)
            .AddParameter("@Description", record.Description)
            .AddParameter("@Data", record.Data)
            .ExecuteNonQuery();

        return result;
    }

    public async Task<Option<int>> UpdateData(string elimCode, string data)
    {
        elimCode.NotEmpty();

        var result = await _client.Query()
            .SetCommand("[App].[UpdateElimOperationData]", CommandType.StoredProcedure)
            .AddParameter("@ElimCode", elimCode)
            .AddParameter("@Data", data)
            .ExecuteNonQuery();

        return result;
    }
}
