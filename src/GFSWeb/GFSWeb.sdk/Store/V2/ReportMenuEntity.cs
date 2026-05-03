using System.Data;
using GFSWeb.sdk;
using GFSWeb.sdk.Models;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Extensions;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Store.V2;

public class ReportMenuEntity
{
    private readonly ISqlClient _client;
    private readonly ILogger _logger;
    private readonly IAuthAccess _authAccess;

    public ReportMenuEntity(ISqlClient client, IAuthAccess authAccess, ILogger logger)
    {
        _client = client.NotNull();
        _authAccess = authAccess.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<Option<int>> Add(MenuRecord subject)
    {
        subject.NotNull().Validate().ThrowOnError();

        var result = await _client.Query()
            .SetCommand("[App].[AddOrUpdateMenu]", CommandType.StoredProcedure)
            .AddParameter("@MenuId", subject.MenuId)
            .AddParameter("@Description", subject.Description)
            .ExecuteNonQuery();

        return result;
    }

    public async Task<Option<int>> Delete(string menuId)
    {
        menuId.NotEmpty();

        var result = await _client.Query()
            .SetCommand("[App].[DeleteMenu]", CommandType.StoredProcedure)
            .AddParameter("@MenuId", menuId)
            .ExecuteNonQuery();

        return result;
    }

    public async Task<IReadOnlyList<MenuRecord>> GetAll()
    {
        var cmd = """
            SELECT  x.*
            FROM    [App].[Menu] x
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .Execute<MenuRecord>();

        return result;
    }

    public async Task<Option<int>> Update(MenuRecord subject)
    {
        subject.NotNull().Validate().ThrowOnError();

        var result = await _client.Query()
            .SetCommand("[App].[UpdateMenu]", CommandType.StoredProcedure)
            .AddParameter("@Id", subject.MenuId)
            .AddParameter("@Description", subject.Description)
            .ExecuteNonQuery();

        return result;
    }

    public async Task<IReadOnlyList<PrincipalMenuRecord>> GetMenuForPrincipalIdentity()
    {
        var nameIdentifier = await _authAccess.GetEmail();
        if (nameIdentifier.IsEmpty()) return [];

        var result = await _client.Query()
            .SetCommand("[App].[GetMenuForPrincipalIdentity]", CommandType.StoredProcedure)
            .AddParameter("@NameIdentifier", nameIdentifier)
            .Execute<PrincipalMenuRecord>();

        return result;
    }
}

