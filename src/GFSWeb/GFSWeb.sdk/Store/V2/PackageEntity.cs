using System.Data;
using GFSWeb.sdk;
using GFSWeb.sdk.Models;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Extensions;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Store.V2;

public class PackageEntity
{
    private readonly ISqlClient _client;
    private readonly ILogger<PackageEntity> _logger;
    private readonly IAuthAccess _authAccess;
    private readonly IStoreNotify? _storeNotify;

    public PackageEntity(ISqlClient client, IAuthAccess authAccess, ILogger<PackageEntity> logger, IStoreNotify? storeNotify = null)
    {
        _client = client.NotNull();
        _authAccess = authAccess.NotNull();
        _logger = logger.NotNull();
        _storeNotify = storeNotify;
    }

    public async Task<Option<ReportPackageRow>> Get(string packageId)
    {
        var nameIdentifier = await _authAccess.GetEmail();
        if (nameIdentifier.IsEmpty()) return StatusCode.Unauthorized;

        var result = await _client.Query()
            .SetCommand("[App].[GetReportPackage]", CommandType.StoredProcedure)
            .AddParameter("@PackageId", packageId)
            .AddParameter("@NameIdentifier", nameIdentifier)
            .Execute<ReportPackageRow>();

        return result.Count switch
        {
            0 => StatusCode.NotFound,
            1 => result[0],
            _ => throw new InvalidOperationException($"Multiple records returned")
        };
    }

    public async Task<IReadOnlyList<ReportPackageRow>> GetAll()
    {
        var nameIdentifier = await _authAccess.GetEmail();
        if (nameIdentifier.IsEmpty()) return Array.Empty<ReportPackageRow>();

        var result = await _client.Query()
            .SetCommand("[App].[GetReportPackage]", CommandType.StoredProcedure)
            .AddParameter("@NameIdentifier", nameIdentifier)
            .Execute<ReportPackageRow>();

        return result;
    }

    public async Task<IReadOnlyList<GroupPackageAccessRow>> GetGroupAccess(string packageId)
    {
        var result = await _client.Query()
            .SetCommand("[App].[GetPackageAccess]", CommandType.StoredProcedure)
            .AddParameter("@PackageId", packageId)
            .Execute<GroupPackageAccessRow>();

        return result;
    }

    public async Task<Option<int>> Upsert(ReportPackageRow record)
    {
        record.NotNull().Validate().ThrowOnError();

        var result = await _client.Query()
            .SetCommand("[App].[UpsertReportPackage]", CommandType.StoredProcedure)
            .AddParameter("@PackageId", record.PackageId)
            .AddParameter("@Description", record.Description)
            .AddParameter("@MenuId", record.MenuId)
            .AddParameter("@Data", record.Data)
            .AddParameter("@Disabled", record.Disabled)
            .ExecuteNonQuery();

        _storeNotify?.Notify(result, $"Added report package {record.PackageId}", $"Failed to add report package {record.PackageId}");
        return result;
    }

    public async Task<Option<int>> Delete(string packageId)
    {
        packageId.NotEmpty();

        var result = await _client.Query()
            .SetCommand("[App].[DeleteReportPackage]", CommandType.StoredProcedure)
            .AddParameter("@PackageId", packageId)
            .ExecuteNonQuery();

        _storeNotify?.Notify(result, $"Deleted report package {packageId}", $"Failed to delete report package {packageId}");
        return result;
    }

    public async Task<Option<int>> ImportFixup()
    {
        var result = await _client.Query()
            .SetCommand("[App].[ImportFixup]", CommandType.StoredProcedure)
            .ExecuteNonQuery();

        return result;
    }

    public async Task<Option<int>> UpdateData(string packageId, string data)
    {
        packageId.NotEmpty();
        data.NotEmpty();

        var result = await _client.Query()
            .SetCommand("[App].[UpdateReportPackageData]", CommandType.StoredProcedure)
            .AddParameter("@PackageId", packageId)
            .AddParameter("@Data", data)
            .ExecuteNonQuery();

        _storeNotify?.Notify(result, $"Updated report package {packageId}", $"Failed to update report package {packageId}");
        return result;
    }
}
