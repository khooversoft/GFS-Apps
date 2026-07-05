using System.Data;
using GFSWeb.sdk.Models;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Extensions;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Store.V2;

public class UserUsageEntity
{
    private readonly ISqlClient _client;
    private readonly ILogger<PrincipalGroupEntity> _logger;
    private readonly IAuthAccess _authAccess;
    private readonly IStoreNotify? _storeNotify;

    public UserUsageEntity(ISqlClient client, IAuthAccess authAccess, ILogger<PrincipalGroupEntity> logger, IStoreNotify? storeNotify = null)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
        _authAccess = authAccess.NotNull();
        _storeNotify = storeNotify;
    }

    public async Task<Option> Clear(string nameIdentifier)
    {
        nameIdentifier.NotEmpty();

        var result = await _client.Query()
            .SetCommand("[App].[ClearUserUsage]", CommandType.StoredProcedure)
            .AddParameter("@NameIdentifier", nameIdentifier)
            .ExecuteNonQuery();

        return result.ToOptionStatus();
    }

    public async Task<IReadOnlyList<PackageUsageRecord>> GetUsage()
    {
        var nameIdentifier = await _authAccess.GetEmail();
        if (nameIdentifier.IsEmpty()) return [];

        var result = await _client.Query()
            .SetCommand("[App].[GetUserUsage]", CommandType.StoredProcedure)
            .AddParameter("@NameIdentifier", nameIdentifier)
            .Execute<PackageUsageRecord>();

        return result;
    }

    public async Task<Option> UpsertUsage(string packageId, bool favorite)
    {
        var nameIdentifier = await _authAccess.GetEmail();
        if (nameIdentifier.IsEmpty()) return StatusCode.Unauthorized;

        var result = await _client.Query()
            .SetCommand("[App].[UpsertPackageUsage]", CommandType.StoredProcedure)
            .AddParameter("@NameIdentifier", nameIdentifier)
            .AddParameter("@PackageId", packageId)
            .AddParameter("@Favorite", favorite)
            .ExecuteNonQuery();

        return result.ToOptionStatus();
    }
}
