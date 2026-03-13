using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Entity;

public class PrincipalIdentityStore
{
    private readonly ISqlClient<PrincipalIdentityStore> _client;
    private readonly ILogger<PrincipalIdentityStore> _logger;

    public PrincipalIdentityStore(ISqlClient<PrincipalIdentityStore> client, ILogger<PrincipalIdentityStore> logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<Option<PrincipalIdentityRecord>> Get(string nameIdentifier)
    {
        var cmd = """
            SELECT  x.*
            FROM    [App].[PrincipalIdentity] x
            WHERE   x.[NameIdentifier] = @NameIdentifier
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .AddParameter("NameIdentifier", nameIdentifier)
            .Execute<PrincipalIdentityRecord>();

        return result.Count switch
        {
            0 => StatusCode.NotFound,
            1 => result[0],
            _ => throw new InvalidOperationException($"Multiple records found for NameIdentifier: {nameIdentifier}")
        };
    }

    public async Task<IReadOnlyList<PrincipalIdentityRecord>> GetAll()
    {
        var cmd = """
            SELECT  x.*
            FROM    [App].[PrincipalIdentity] x
            ORDER BY x.[NameIdentifier]
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .Execute<PrincipalIdentityRecord>();

        return result;
    }

    public async Task<IReadOnlyList<PrincipalRole>> GetRoles(string nameIdentifier)
    {
        var cmd = """
            SELECT  x.[RoleCode]
            FROM    [App].[PrincipalRole] x
            WHERE   x.[NameIdentifier] = @NameIdentifier
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .AddParameter("@NameIdentifier", nameIdentifier)
            .Execute<PrincipalRole>();

        return result;
    }

    public async Task<Option<int>> Add(PrincipalIdentityRecord record)
    {
        record.NotNull().Validate().ThrowOnError();

        var result = await _client.Query()
            .SetCommand("[App].[AddPrincipalIdentity]", CommandType.StoredProcedure)
            .AddParameter("@NameIdentifier", record.NameIdentifier)
            .AddParameter("@UserName", record.UserName)
            .AddParameter("@Email", record.Email)
            .AddParameter("@Disabled", record.Disabled)
            .AddParameter("@Role", record.Role)
            .AddParameter("@Parker", record.Parker)
            .AddParameter("@ParkerPost", record.ParkerPost)
            .ExecuteNonQuery();

        return result;
    }
    public async Task<Option<int>> Update(PrincipalIdentityRecord record)
    {
        record.NotNull().Validate().ThrowOnError();

        var result = await _client.Query()
            .SetCommand("[App].[UpdatePrincipalIdentity]", CommandType.StoredProcedure)
            .AddParameter("@NameIdentifier", record.NameIdentifier)
            .AddParameter("@UserName", record.UserName)
            .AddParameter("@Email", record.Email)
            .AddParameter("@Disabled", record.Disabled)
            .AddParameter("@Role", record.Role)
            .AddParameter("@Parker", record.Parker)
            .AddParameter("@ParkerPost", record.ParkerPost)
            .ExecuteNonQuery();

        return result;
    }

    public async Task<Option<int>> Delete(string nameIdentifier)
    {
        nameIdentifier.NotEmpty();

        var result = await _client.Query()
            .SetCommand("[App].[DeletePrincipalIdentity]", CommandType.StoredProcedure)
            .AddParameter("@NameIdentifier", nameIdentifier)
            .ExecuteNonQuery();

        return result;
    }
}
