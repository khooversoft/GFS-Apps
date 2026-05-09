using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using GFSWeb.sdk.Models;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Extensions;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Store.V2;

public class CommandEntity
{
    private readonly ISqlClient _client;
    private readonly ILogger _logger;

    public CommandEntity(ISqlClient client, ILogger logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<Option<CommandRecord>> Get(string commandId)
    {
        var result = await _client.Query()
            .SetCommand("[App].[GetCommand]", CommandType.StoredProcedure)
            .AddParameter("@CommandId", commandId)
            .Execute<CommandRecord>();

        return result.Count switch
        {
            0 => StatusCode.NotFound,
            1 => result[0],
            _ => throw new InvalidOperationException($"Multiple records returned")
        };
    }

    public async Task<IReadOnlyList<CommandRecord>> GetAll()
    {
        var result = await _client.Query()
            .SetCommand("[App].[GetCommand]", CommandType.Text)
            .Execute<CommandRecord>();

        return result;
    }

    public async Task<Option<int>> Upsert(CommandRecord record)
    {
        record.NotNull().Validate().ThrowOnError();

        var result = await _client.Query()
            .SetCommand("[App].[UpsertCommand]", CommandType.StoredProcedure)
            .AddParameter("@CommandId", record.CommandId)
            .AddParameter("@Description", record.Description)
            .AddParameter("@Data", record.Data)
            .AddParameter("@Disabled", record.Disabled)
            .ExecuteNonQuery();

        return result;
    }

    public async Task<Option<int>> Delete(string commandId)
    {
        commandId.NotEmpty();

        var result = await _client.Query()
            .SetCommand("[App].[DeleteCommand]", CommandType.StoredProcedure)
            .AddParameter("@CommandId", commandId)
            .ExecuteNonQuery();

        return result;
    }
}
