using System.Data;
using GFSWeb.sdk.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Store.V2;

public class PrincipalGroupEntity
{
    private readonly ISqlClient _client;
    private readonly ILogger _logger;
    private readonly IStoreNotify? _storeNotify;

    public PrincipalGroupEntity(ISqlClient client, IStoreNotify? storeNotify, ILogger logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
        _storeNotify = storeNotify;
    }

    public async Task<Option<PrincipalGroupRecord>> Get(string groupName)
    {
        groupName.NotEmpty();
        var result = await _client.Query()
            .SetCommand("[App].[GetPrincipalGroup]", CommandType.StoredProcedure)
            .AddParameter("@GroupName", groupName)
            .Execute<PrincipalGroupRecord>();

        return result.Count switch
        {
            0 => StatusCode.NotFound,
            1 => result[0],
            _ => throw new InvalidOperationException($"Multiple records found for GroupName: {groupName}")
        };
    }

    public async Task<IReadOnlyList<PrincipalGroupRecord>> GetAll()
    {
        var result = await _client.Query()
            .SetCommand("[App].[GetPrincipalGroups]", CommandType.StoredProcedure)
            .Execute<PrincipalGroupRecord>();

        return result;
    }

    public async Task<Option<int>> Delete(string groupName)
    {
        groupName.NotEmpty();

        var result = await _client.Query()
            .SetCommand("[App].[DeletePrincipalGroup]", CommandType.StoredProcedure)
            .AddParameter("@GroupName", groupName)
            .ExecuteNonQuery();

        _storeNotify?.Notify(result, $"Deleted principal group {groupName}", $"Failed to delete principal group {groupName}");
        return result;
    }

    public async Task<Option<int>> Upsert(string groupName, string description)
    {
        groupName.NotEmpty();
        description.NotEmpty();

        var result = await _client.Query()
            .SetCommand("[App].[UpsertPrincipalGroup]", CommandType.StoredProcedure)
            .AddParameter("@GroupName", groupName)
            .AddParameter("@Description", description)
            .ExecuteNonQuery();

        _storeNotify?.Notify(result, $"Updated principal group {groupName}", $"Failed to update principal group {groupName}");
        return result;
    }

    public async Task<IReadOnlyList<PrincipalGroupMembershipRecord>> GetGroupMembership(string groupName)
    {
        groupName.NotEmpty();

        var result = await _client.Query()
            .SetCommand("[App].[GetGroupMembership]", CommandType.StoredProcedure)
            .AddParameter("@GroupName", groupName)
            .Execute<PrincipalGroupMembershipRecord>(factory);

        return result;

        static IReadOnlyList<PrincipalGroupMembershipRecord> factory(SqlDataReader reader)
        {
            var groups = reader.MapToList<PrincipalGroupRecord>().Assert(x => x.Count == 1, "Multiple groups returned");
            reader.NextResult();
            var identities = reader.MapToList<PrincipalIdentityRecord>();

            return identities.Select(x => new PrincipalGroupMembershipRecord
            {
                Group = groups[0],
                Identity = x,
            }).ToList();
        }
    }

    public async Task<Option<int>> DeleteGroupMembership(string groupName, string nameIdentifier)
    {
        groupName.NotEmpty();
        nameIdentifier.NotEmpty();

        var result = await _client.Query()
            .SetCommand("[App].[DeleteGroupMembership]", CommandType.StoredProcedure)
            .AddParameter("@GroupName", groupName)
            .AddParameter("@NameIdentifier", nameIdentifier)
            .ExecuteNonQuery();

        _storeNotify?.Notify(
            result,
            $"Deleted principal group membership {nameIdentifier} from {groupName}",
            $"Failed to delete principal group membership {nameIdentifier} from {groupName}"
            );

        return result;
    }

    public async Task<Option<int>> UpsertGroupMembership(string groupName, string nameIdentifier)
    {
        groupName.NotEmpty();
        nameIdentifier.NotEmpty();

        var result = await _client.Query()
            .SetCommand("[App].[UpsertGroupMembership]", CommandType.StoredProcedure)
            .AddParameter("@GroupName", groupName)
            .AddParameter("@NameIdentifier", nameIdentifier)
            .ExecuteNonQuery();

        _storeNotify?.Notify(
            result,
            $"Updated principal group membership {nameIdentifier} in {groupName}",
            $"Failed to update principal group membership {nameIdentifier} in {groupName}"
            );

        return result;
    }
}
