using System.Data;
using GFSWeb.sdk.Models;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Store.V2;

public class CommandEntity
{
    private readonly ISqlClient _client;
    private readonly ILogger _logger;
    private readonly IStoreNotify? _storeNotify;

    public CommandEntity(ISqlClient client, IStoreNotify? storeNotify, ILogger logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
        _storeNotify = storeNotify;
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
            .AddParameter("@Type", record.Type)
            .AddParameter("@Data", record.Data)
            .AddParameter("@Hash", record.Hash)
            .AddParameter("@Disabled", record.Disabled)
            .ExecuteNonQuery();

        _storeNotify?.Notify(result, $"Updated command {record.Description}", $"Failed to update command {record.Description}");
        return result;
    }

    public async Task<Option<int>> Delete(string commandId, string description)
    {
        commandId.NotEmpty();
        description.NotEmpty();

        var result = await _client.Query()
            .SetCommand("[App].[DeleteCommand]", CommandType.StoredProcedure)
            .AddParameter("@CommandId", commandId)
            .ExecuteNonQuery();

        _storeNotify?.Notify(result, $"Deleted command {description}", $"Failed to delete command {description}, CommandId={commandId}");
        return result;
    }
}
