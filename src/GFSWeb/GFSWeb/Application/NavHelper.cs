using Microsoft.AspNetCore.Components;

using Toolbox.Extensions;
using Toolbox.Tools;

namespace GFSWeb.Application;

public static class NavHelper
{
    public static string AccessManagement => "/access";
    public static string UnderConstructionPath => "/underConstruction";
    public static string ElimPackagesPath => "/elimPackages";
    public static string ElimPackagePath => "/elimPackage";
    public static string EditElimPackagePath => "/editElimPackage";

    public static void GotoAcessManagement(this NavigationManager nav) => nav.NotNull().NavigateTo(AccessManagement);
    public static void GotoElimPackages(this NavigationManager nav) => nav.NotNull().NavigateTo(ElimPackagesPath);

    //public static void GotoCalendar(this NavigationManager nav) => nav.NotNull().NavigateTo(MarathonCalendarPath);

    public static string GetPrincipalHref(string id, string? returnUrl = null)
    {
        id = Uri.EscapeDataString(id);

        var url = Build([AccessManagement, id], BuildReturnUrl(returnUrl));
        return $"{NavHelper.AccessManagement}/{id}?returnUrl={returnUrl}";
    }

    public static void GotoPrincipalEditPage(this NavigationManager nav, string id, string? returnUrl = null)
    {
        var url = GetPrincipalHref(id, returnUrl);
        nav.NotNull().NavigateTo(url);
    }

    public static string GetElimPackageHref(string packageId, string? returnUrl = null)
    {
        packageId = Uri.EscapeDataString(packageId);
        var url = Build([ElimPackagePath, packageId], BuildReturnUrl(returnUrl));
        return url;
    }

    public static void GotoElimPackageHref(this NavigationManager nav, string packageId, string? returnUrl = null)
    {
        var url = GetElimPackageHref(packageId, returnUrl);
        nav.NotNull().NavigateTo(url);
    }

    public static string GetEditElimReportHref(string packageId, string? returnUrl = null)
    {
        packageId = Uri.EscapeDataString(packageId);
        var url = Build([EditElimPackagePath, packageId], BuildReturnUrl(returnUrl));
        return url;
    }

    public static void GotoEditElimReportHref(this NavigationManager nav, string packageId, string? returnUrl = null)
    {
        var url = GetEditElimReportHref(packageId, returnUrl);
        nav.NotNull().NavigateTo(url);
    }

    private static string? BuildReturnUrl(string? url) => url.IsEmpty() ? null : $"returnUrl={url}";

    private static string Build(IEnumerable<string> paths, string? query) => Build(paths, query.IsNotEmpty() ? [query] : null);

    private static string Build(IEnumerable<string> paths, IEnumerable<string>? queries = null)
    {
        var path = paths.NotNull().Where(x => x.IsNotEmpty()).Join("/").NotEmpty();

        string url = queries switch
        {
            IEnumerable<string> q when q.Any() => $"{path}?{q.Where(x => x.IsNotEmpty()).Join("&")}",
            _ => path,
        };

        return url;
    }
}
