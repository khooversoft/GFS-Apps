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
    public static readonly NavRoute PrincipalAccess = new("/access");
    public static readonly NavRoute UnderConstruction = new("/underConstruction");
    public static readonly NavRoute ElimPackages = new("/elimPackages");
    public static readonly NavRoute ElimPackage = new("/elimPackage");
    public static readonly NavRoute EditElimPackage = new("/editElimPackage");

    public static void GotoAccessManagement(this NavigationManager nav) => PrincipalAccess.NavigateTo(nav);
    public static void GotoElimPackages(this NavigationManager nav) => ElimPackages.NavigateTo(nav);

    public static string GetPrincipalHref(string id, string? returnUrl = null) => PrincipalAccess.Href(id, returnUrl);
    public static string GetElimPackageHref(string packageId, string? returnUrl = null) => ElimPackage.Href(packageId, returnUrl);
    public static string GetEditElimReportHref(string packageId, string? returnUrl = null) => EditElimPackage.Href(packageId, returnUrl);

    public static void GotoPrincipalEditPage(this NavigationManager nav, string id, string? returnUrl = null)
        => PrincipalAccess.NavigateTo(nav, id, returnUrl);

    public static void GotoElimPackageHref(this NavigationManager nav, string packageId, string? returnUrl = null)
        => ElimPackage.NavigateTo(nav, packageId, returnUrl);

    public static void GotoEditElimReportHref(this NavigationManager nav, string packageId, string? returnUrl = null)
        => EditElimPackage.NavigateTo(nav, packageId, returnUrl);
}
