using Microsoft.AspNetCore.Components;

using Toolbox.Extensions;
using Toolbox.Tools;

namespace GFSWeb.Application;

/// <summary>
/// Represents a single application route and owns its URL construction logic.
/// </summary>
public sealed record NavRoute(string BasePath)
{
    /// <summary>Builds a URL, optionally appending a path segment and/or a returnUrl query parameter.</summary>
    public string Href(string? segment = null, string? returnUrl = null)
    {
        var parts = new[] { BasePath, segment.IsNotEmpty() ? Uri.EscapeDataString(segment!) : null }
            .Where(x => x.IsNotEmpty());

        string path = parts.Join("/").NotEmpty();

        return returnUrl.IsEmpty()
            ? path
            : $"{path}?returnUrl={Uri.EscapeDataString(returnUrl!)}";
    }

    /// <summary>Navigates to this route via the given NavigationManager.</summary>
    public void NavigateTo(NavigationManager nav, string? segment = null, string? returnUrl = null)
        => nav.NotNull().NavigateTo(Href(segment, returnUrl));
}

public static class NavHelper
{
    public static readonly NavRoute Principals = new("/principals");
    public static readonly NavRoute Principal = new("/principal");
    public static readonly NavRoute PrincipalGroups = new("/principalGroups");
    public static readonly NavRoute PrincipalGroup = new("/principalGroup");
    public static readonly NavRoute PrincipalAccessMembership = new("/principalGroup/access");
    public static readonly NavRoute UnderConstruction = new("/underConstruction");
    public static readonly NavRoute ElimPackages = new("/elimPackages");
    public static readonly NavRoute ElimPackage = new("/elimPackage");
    public static readonly NavRoute EditElimPackage = new("/editElimPackage");
    public static readonly NavRoute Help = new("/help");
    public static readonly NavRoute Commands = new("/commands");
    public static readonly NavRoute Command = new("/command");
    public static readonly NavRoute Packages = new("/packages");
    public static readonly NavRoute Package = new("/package");
}
