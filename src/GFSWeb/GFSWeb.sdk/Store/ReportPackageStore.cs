using System.Data;
using GFSWeb.sdk.Models;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Store;

public class ReportPackageStore
{
    private readonly ISqlClient<ReportPackageStore> _client;
    private readonly ILogger<ReportPackageStore> _logger;

    public ReportPackageStore(ISqlClient<ReportPackageStore> client, ILogger<ReportPackageStore> logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();

        Access = new(_client, logger);
        Menu = new(_client, logger);
    }

    public ReportAccessStore Access { get; }
    public ReportMenuStore Menu { get; }

    public async Task<Option<ReportPackageRecord>> Get(string packageId)
    {
        var cmd = """
            SELECT  x.*
            FROM    [App].[ReportPackage] x
            WHERE   x.[[PackageId]] = @PackageId
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .AddParameter("@PackageId", packageId)
            .Execute<ReportPackageRecord>();

        return result.Count switch
        {
            0 => StatusCode.NotFound,
            1 => result[0],
            _ => throw new InvalidOperationException($"Multiple records found for NameIdentifier: {packageId}")
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

        return result;
    }

    public async Task<Option<int>> Delete(string packageId)
    {
        packageId.NotEmpty();

        var result = await _client.Query()
            .SetCommand("[App].[DeleteReportPackage]", CommandType.StoredProcedure)
            .AddParameter("@PackageId", packageId)
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

        return result;
    }
}
