using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using GFSWeb.sdk.Models;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Store;

public class ReportAccessStore
{
    private readonly ISqlClient _client;
    private readonly ILogger _logger;
    private readonly IAuthAccess _authAccess;

    public ReportAccessStore(ISqlClient client, IAuthAccess authAccess, ILogger<ReportPackageStore> logger)
    {
        _client = client.NotNull();
        _authAccess = authAccess.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<Option<int>> Add(ReportAccessRecord subject)
    {
        subject.NotNull().Validate().ThrowOnError();

        var result = await _client.Query()
            .SetCommand("[App].[AddReportAccess]", CommandType.StoredProcedure)
            .AddParameter("@PackageId", subject.PackageId)
            .AddParameter("@NameIdentifier", subject.NameIdentifier)
            .AddParameter("@Access", subject.Access)
            .ExecuteNonQuery();

        return result;
    }

    public async Task<IReadOnlyList<ReportAccessRecord>> GetAll()
    {
        var cmd = """
            SELECT  x.*
            FROM    [App].[ReportAccess] x
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .Execute<ReportAccessRecord>();

        return result;
    }

    public async Task<IReadOnlyList<ReportAccessRecord>> GetByPackageId(string reportId)
    {
        reportId.NotEmpty();

        var cmd = """
            SELECT  x.*
            FROM    [App].[ReportAccess] x
            WHERE   x.[PackageId] = @PackageId
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .AddParameter("@PackageId", reportId)
            .Execute<ReportAccessRecord>();

        return result;
    }

    public async Task<Option<int>> Delete(string reportId, string nameIdentifier)
    {
        reportId.NotEmpty();
        nameIdentifier.NotEmpty();

        var result = await _client.Query()
            .SetCommand("[App].[DeleteReportAccess]", CommandType.StoredProcedure)
            .AddParameter("@PackageId", reportId)
            .AddParameter("@NameIdentifier", nameIdentifier)
            .ExecuteNonQuery();

        return result;
    }
}
