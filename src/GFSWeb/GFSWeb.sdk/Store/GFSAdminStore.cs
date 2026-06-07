using GFSWeb.sdk.Store.V2;
using Microsoft.Extensions.DependencyInjection;
using Toolbox.Data;
using Toolbox.Tools;

namespace GFSWeb.sdk.Store;

public class GFSAdminStore
{
    public GFSAdminStore(ISqlClient<GFSAdminStore> client, IServiceProvider serviceProvider)
    {
        client.NotNull();
        serviceProvider.NotNull();

        Menu = ActivatorUtilities.CreateInstance<ReportMenuEntity>(serviceProvider, client);
        Identity = ActivatorUtilities.CreateInstance<PrincipalIdentityEntity>(serviceProvider, client);
        UserAccess = ActivatorUtilities.CreateInstance<UserAccessEntity>(serviceProvider, client);
        PrincipalGroup = ActivatorUtilities.CreateInstance<PrincipalGroupEntity>(serviceProvider, client);
        Package = ActivatorUtilities.CreateInstance<PackageEntity>(serviceProvider, client);
        Command = ActivatorUtilities.CreateInstance<CommandEntity>(serviceProvider, client);
    }

    public ReportMenuEntity Menu { get; }
    public PrincipalIdentityEntity Identity { get; }
    public UserAccessEntity UserAccess { get; }
    public PrincipalGroupEntity PrincipalGroup { get; }
    public PackageEntity Package { get; }
    public CommandEntity Command { get; }
}
