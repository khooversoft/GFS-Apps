using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Entity;

public class AppRoleStore
{
    private readonly ISqlClient<AppRoleStore> _client;
    private readonly ILogger<AppRoleStore> _logger;

    public AppRoleStore(ISqlClient<AppRoleStore> client, ILogger<AppRoleStore> logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<IReadOnlyList<AppRoleRecord>> GetAll()
    {
        var cmd = """
            SELECT  x.[RoleCode]
                    ,x.[Description]
            FROM    [App].[AppRole] x
            """;

        var result = await _client.Query()
            .SetCommand(cmd, CommandType.Text)
            .Execute<AppRoleRecord>();

        return result;
    }
}

