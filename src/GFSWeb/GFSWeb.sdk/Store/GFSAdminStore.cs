using System;
using System.Collections.Generic;
using System.Text;
using GFSWeb.sdk.Store.V2;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;

namespace GFSWeb.sdk.Store;

public class GFSAdminStore
{
    private readonly ISqlClient _client;
    private readonly IAuthAccess _authAccess;
    private readonly ILogger _logger;

    public GFSAdminStore(ISqlClient<GFSAdminStore> client, IAuthAccess authAccess, ILogger<GFSAdminStore> logger)
    {
        _client = client.NotNull();
        _authAccess = authAccess.NotNull();
        _logger = logger.NotNull();

        // TODO: Hook up auth access
        //Access = new(_client, _authAccess, logger);
        Menu = new(_client, _authAccess, logger);
        Identity = new(_client, logger);
        UserAccess = new(_client, logger);
        PrincipalGroup = new(_client, logger);
        Package = new(_client, _authAccess, logger);
    }

    public ReportMenuEntity Menu { get; }
    public PrincipalIdentityEntity Identity { get; }
    public UserAccessEntity UserAccess { get; }
    public PrincipalGroupEntity PrincipalGroup { get; }
    public ReportPackageEntity Package { get; }
}
