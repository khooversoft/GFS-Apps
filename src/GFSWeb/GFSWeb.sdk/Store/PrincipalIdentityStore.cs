using System.Data;
using GFSWeb.sdk.Models;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Store;

public class PrincipalIdentityStore
{
    private readonly ISqlClient<PrincipalIdentityStore> _client;
    private readonly ILogger<PrincipalIdentityStore> _logger;

    public PrincipalIdentityStore(ISqlClient<PrincipalIdentityStore> client, ILogger<PrincipalIdentityStore> logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();

        UserAccess = new UserAccessStore(client, logger);
    }

    public UserAccessStore UserAccess { get; }

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

    public async Task<Option<int>> AddOrUpdate(PrincipalIdentityRecord record)
    {
        record.NotNull().Validate().ThrowOnError();

        var result = await _client.Query()
            .SetCommand("[App].[UpdateOrAddPrincipalIdentity]", CommandType.StoredProcedure)
            .AddParameter("@NameIdentifier", record.NameIdentifier)
            .AddParameter("@UserName", record.UserName)
            .AddParameter("@Email", record.Email)
            .AddParameter("@Disabled", record.Disabled)
            .AddParameter("@Role", record.Role)
            .AddParameter("@Parker", record.Parker)
            .AddParameter("@ParkerPost", record.ParkerPost)
            .AddParameter("@UserID_SAP", record.UserID_SAP)
            .AddParameter("@FirstName", record.FirstName)
            .AddParameter("@NickName", record.NickName)
            .AddParameter("@MiddleName", record.MiddleName)
            .AddParameter("@LastName", record.LastName)
            .AddParameter("@Location", record.Location)
            .AddParameter("@ParkPost", record.ParkPost)
            .AddParameter("@PostEmail", record.PostEmail)
            .AddParameter("@CCEmail1", record.CCEmail1)
            .AddParameter("@CCEmail2", record.CCEmail2)
            .AddParameter("@Elim", record.Elim)
            .AddParameter("@Co_Update", record.Co_Update)
            .AddParameter("@Co_View", record.Co_View)
            .AddParameter("@CC_NodeID", record.CC_NodeID)
            .AddParameter("@Flex1", record.Flex1)
            .AddParameter("@Flex2", record.Flex2)
            .AddParameter("@Flex3", record.Flex3)
            .AddParameter("@PostEmail2", record.PostEmail2)
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
