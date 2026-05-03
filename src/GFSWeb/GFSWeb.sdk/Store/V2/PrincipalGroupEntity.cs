using System.Data;
using GFSWeb.sdk.Models;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Store.V2;

public class PrincipalGroupEntity
{
    private readonly ISqlClient _client;
    private readonly ILogger _logger;

    public PrincipalGroupEntity(ISqlClient client, ILogger logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<Option<int>> Add(PrincipalGroupRecord subject)
    {
        subject.NotNull().Validate().ThrowOnError();

        var result = await _client.Query()
            .SetCommand("[App].[AddPrincipalGroup]", CommandType.StoredProcedure)
            .AddParameter("@GroupName", subject.GroupName)
            .AddParameter("@Description", subject.Description)
            .ExecuteNonQuery();

        return result;
    }

    public async Task<IReadOnlyList<PrincipalGroupRecord>> Get()
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

        return result;
    }

    public async Task<IReadOnlyList<PrincipalGroupRecord>> GetGroupMembership(string groupName)
    {
        groupName.NotEmpty();

        var result = await _client.Query()
            .SetCommand("[App].[GetGroupMembership]", CommandType.StoredProcedure)
            .AddParameter("@GroupName", groupName)
            .Execute<PrincipalGroupRecord>();

        return result;
    }
}
