using GFSWeb.sdk.Store.V2;
using Microsoft.Extensions.Logging;
using Toolbox.Data;
using Toolbox.Tools;

namespace GFSWeb.sdk.Store;

public class GFSAdminStore
{
    private readonly ISqlClient _client;
    private readonly IAuthAccess _authAccess;

    public GFSAdminStore(ISqlClient<GFSAdminStore> client, IAuthAccess authAccess, IStoreNotify storeNotify, ILogger<GFSAdminStore> logger)
    {
        _client = client.NotNull();
        _authAccess = authAccess.NotNull();

        // TODO: Hook up auth access
        //Access = new(_client, _authAccess, logger);
        Menu = new(_client, _authAccess, storeNotify, logger);
        Identity = new(_client, storeNotify, logger);
        UserAccess = new(_client, storeNotify, logger);
        PrincipalGroup = new(_client, storeNotify, logger);
        Package = new(_client, _authAccess, storeNotify, logger);
        Command = new(_client, storeNotify, logger);
    }

    public GFSAdminStore(ISqlClient<GFSAdminStore> client, IAuthAccess authAccess, ILogger<GFSAdminStore> logger)
    {
        _client = client.NotNull();
        _authAccess = authAccess.NotNull();

        // TODO: Hook up auth access
        //Access = new(_client, _authAccess, logger);
        Menu = new(_client, _authAccess, null, logger);
        Identity = new(_client, null, logger);
        UserAccess = new(_client, null, logger);
        PrincipalGroup = new(_client, null, logger);
        Package = new(_client, _authAccess, null, logger);
        Command = new(_client, null, logger);
    }

    public ReportMenuEntity Menu { get; }
    public PrincipalIdentityEntity Identity { get; }
    public UserAccessEntity UserAccess { get; }
    public PrincipalGroupEntity PrincipalGroup { get; }
    public PackageEntity Package { get; }
    public CommandEntity Command { get; }
}
