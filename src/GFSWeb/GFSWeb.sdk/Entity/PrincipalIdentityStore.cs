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

    public async Task<IReadOnlyList<PrincipalIdentityRecord>> GetAll()
    {
        var cmd = """
            SELECT  x.[NameIdentifier]
                    ,x.[UserName]
                    ,x.[Email]
                    ,x.[Disabled]
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

    public async Task<Option<int>> AddOrUpdate(PrincipalIdentityRecord record)
    {
        record.NotNull().Validate().ThrowOnError();

        var result = await _client.Query()
            .SetCommand("EXEC [App].[AddOrUpdatePrincipalIdentity] @NameIdentifier, @UserName, @Email, @Disabled", CommandType.StoredProcedure)
            .AddParameter("@NameIdentifier", record.NameIdentifier)
            .AddParameter("@UserName", record.UserName)
            .AddParameter("@Email", record.Email)
            .AddParameter("@Disabled", record.Disabled)
            .ExecuteNonQuery();

        return result;
    }

    public async Task<Option<int>> Delete(string nameIdentifier)
    {
        nameIdentifier.NotEmpty();

        var result = await _client.Query()
            .SetCommand("EXEC [App].[DeletePrincipalIdentity] @NameIdentifier", CommandType.StoredProcedure)
            .AddParameter("@NameIdentifier", nameIdentifier)
            .ExecuteNonQuery();

        return result;
    }
}
