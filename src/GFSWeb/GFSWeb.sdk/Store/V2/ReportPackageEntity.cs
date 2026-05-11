using System.Data;
using GFSWeb.sdk;
using GFSWeb.sdk.Models;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Extensions;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Store.V2;

public class ReportPackageEntity
{
    private readonly ISqlClient _client;
    private readonly ILogger _logger;
    private readonly IAuthAccess _authAccess;
    private readonly IStoreNotify? _storeNotify;

    public ReportPackageEntity(ISqlClient client, IAuthAccess authAccess, IStoreNotify? storeNotify, ILogger logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
        _authAccess = authAccess.NotNull();
        _storeNotify = storeNotify;
    }

    public async Task<Option<ReportPackageRecord>> Get(string packageId)
    {
        var nameIdentifier = await _authAccess.GetEmail();
        if (nameIdentifier.IsEmpty()) return StatusCode.Unauthorized;

        var result = await _client.Query()
            .SetCommand("[App].[GetReportPackage]", CommandType.StoredProcedure)
            .AddParameter("@PackageId", packageId)
            .AddParameter("@NameIdentifier", nameIdentifier)
            .Execute<ReportPackageRecord>();

        return result.Count switch
        {
            0 => StatusCode.NotFound,
            1 => result[0],
            _ => throw new InvalidOperationException($"Multiple records returned")
        };
    }

    public async Task<IReadOnlyList<ReportPackageRecord>> GetAll()
    {
        var cmd = """
            SELECT  x.*
            FROM    [App].[ReportPackage] x
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .Execute<ReportPackageRecord>();

        return result;
    }

    public async Task<IReadOnlyList<GroupPackageAccessRecord>> GetGroupAccess(string packageId)
    {
        var result = await _client.Query()
            .SetCommand("[App].[GetPackageAccess]", CommandType.StoredProcedure)
            .AddParameter("@PackageId", packageId)
            .Execute<GroupPackageAccessRecord>();

        return result;
    }

    public async Task<Option<int>> AddOrUpdate(ReportPackageRecord record)
    {
        record.NotNull().Validate().ThrowOnError();

        var result = await _client.Query()
            .SetCommand("[App].[AddOrUpdateReportPackage]", CommandType.StoredProcedure)
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

    public async Task<Option<int>> Update(ReportPackageRecord record)
    {
        record.NotNull().Validate().ThrowOnError();

        var result = await _client.Query()
            .SetCommand("[App].[UpdateReportPackage]", CommandType.StoredProcedure)
            .AddParameter("@PackageId", record.PackageId)
            .AddParameter("@Description", record.Description)
            .AddParameter("@ParentPackageId", record.MenuId)
            .AddParameter("@Data", record.Data)
            .AddParameter("@Disabled", record.Disabled)
            .ExecuteNonQuery();

        _storeNotify?.Notify(result, $"Updated report package {record.PackageId}", $"Failed to update report package {record.PackageId}");
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
