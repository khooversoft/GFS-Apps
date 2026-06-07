using System.Data;
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
    private readonly ILogger<CommandEntity> _logger;
    private readonly IStoreNotify? _storeNotify;
    private readonly ICacheClient<CommandRecord> _cacheClient;

    public CommandEntity(ISqlClient client, ICacheClient<CommandRecord> cacheClient, ILogger<CommandEntity> logger, IStoreNotify? storeNotify = null)
    {
        _client = client.NotNull();
        _cacheClient = cacheClient.NotNull();
        _logger = logger.NotNull();
        _storeNotify = storeNotify;
    }

    public async Task<Option<CommandRecord>> Get(string commandId)
    {
        if (_cacheClient.TryGetValue(commandId, out var cached)) return cached.NotNull();

        var result = await _client.Query()
            .SetCommand("[App].[GetCommand]", CommandType.StoredProcedure)
            .AddParameter("@CommandId", commandId)
            .Execute<CommandRecord>();

        return result.Count switch
        {
            0 => StatusCode.NotFound,
            1 => result[0].Action(x => AddToCache(x)),
            _ => throw new InvalidOperationException($"Multiple records returned")
        };
    }

    public async Task<Option<CommandRecord>> GetByHash(string hash)
    {
        hash.NotEmpty();
        if (_cacheClient.TryGetValue(hash, out var cached)) return cached.NotNull();

        var result = await _client.Query()
            .SetCommand("[App].[GetCommand]", CommandType.StoredProcedure)
            .AddParameter("@Hash", hash)
            .Execute<CommandRecord>();

        return result.Count switch
        {
            0 => StatusCode.NotFound,
            1 => result[0].Action(x => AddToCache(x)),
            _ => throw new InvalidOperationException($"Multiple records returned")
        };
    }

    public async Task<IReadOnlyList<CommandRecord>> GetAll()
    {
        var result = await _client.Query()
            .SetCommand("[App].[GetCommand]", CommandType.Text)
            .Execute<CommandRecord>();

        foreach (var item in result) AddToCache(item);
        return result;
    }

    public async Task<Option<int>> Upsert(CommandRecord record)
    {
        record.NotNull().Validate().ThrowOnError();
        _cacheClient.Remove(record.CommandId);
        _cacheClient.Remove(record.CommandId);

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

        if (_cacheClient.TryGetValue(commandId, out var cached))
            RemoveFromCache(cached);
        else
            _cacheClient.Remove(commandId);

        var result = await _client.Query()
            .SetCommand("[App].[DeleteCommand]", CommandType.StoredProcedure)
            .AddParameter("@CommandId", commandId)
            .ExecuteNonQuery();

        _storeNotify?.Notify(result, $"Deleted command {description}", $"Failed to delete command {description}, CommandId={commandId}");
        return result;
    }

    private void AddToCache(CommandRecord record)
    {
        _cacheClient.Upsert(record.CommandId, record);
        _cacheClient.Upsert(record.Hash, record);
    }

    private void RemoveFromCache(CommandRecord record)
    {
        _cacheClient.Remove(record.CommandId);
        _cacheClient.Remove(record.Hash);
    }
}
