using System.Data;
using GFSWeb.sdk.Models;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Store;

public class PrincipalGroupStore
{
    private readonly ISqlClient _client;
    private readonly ILogger _logger;

    public PrincipalGroupStore(ISqlClient client, ILogger logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<Option<int>> Add(string groupName)
    {
        groupName.NotEmpty();

        var result = await _client.Query()
            .SetCommand("[App].[AddPrincipalGroup]", CommandType.StoredProcedure)
            .AddParameter("@GroupName", groupName)
            .ExecuteNonQuery();

        return result;
    }

    public async Task<IReadOnlyList<PrincipalGroupRecord>> Get()
    {
        var result = await _client.Query()
            .SetCommand("[dbo].[GetPrincipalGroups]", CommandType.StoredProcedure)
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
}
