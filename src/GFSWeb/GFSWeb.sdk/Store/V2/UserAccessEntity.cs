using System.Data;
using GFSWeb.sdk;
using GFSWeb.sdk.Models;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Store.V2;

public class UserAccessEntity
{
    private readonly ISqlClient _client;
    private readonly ILogger _logger;
    private readonly IStoreNotify? _storeNotify;

    public UserAccessEntity(ISqlClient client, IStoreNotify? storeNotify, ILogger logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
        _storeNotify = storeNotify;
    }

    public async Task<Option<int>> AddOrUpdate(UserAccessRecord record)
    {
        record.NotNull().Validate().ThrowOnError();

        var result = await _client.Query()
            .SetCommand("[App].[AddOrUpdateUserAccess]", CommandType.StoredProcedure)
            .AddParameter("@PrincipalNameIdentity", record.PrincipalNameIdentity)
            .AddParameter("@Table_ID", record.Table_ID)
            .AddParameter("@ID", record.ID)
            .AddParameter("@Descr", record.Descr)
            .AddParameter("@Field1", record.Field1)
            .AddParameter("@Field2", record.Field2)
            .AddParameter("@Field3", record.Field3)
            .AddParameter("@Field4", record.Field4)
            .AddParameter("@Field5", record.Field5)
            .AddParameter("@Field6", record.Field6)
            .ExecuteNonQuery();

        _storeNotify?.Notify(
            result,
            $"Added user's access for principal {record.PrincipalNameIdentity}",
            $"Failed to add user's access for principal {record.PrincipalNameIdentity}"
            );

        return result;
    }

    public async Task<IReadOnlyList<UserAccessRecord>> Get()
    {
        var cmd = """
            SELECT  x.*
            FROM    [App].[UserAccess] x
            ORDER BY x.[UserID], x.[ResourceID]
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .Execute<UserAccessRecord>();

        return result;
    }

    public async Task<Option<int>> Delete(int userId)
    {
        var result = await _client.Query()
            .SetCommand("[App].[DeleteUserAccess]", CommandType.StoredProcedure)
            .AddParameter("@UserID", userId)
            .ExecuteNonQuery();

        _storeNotify?.Notify(result, $"Delete user's access for userId {userId}", $"Failed to delete user's access for userId {userId}");
        return result;
    }
}
