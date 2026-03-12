using Microsoft.AspNetCore.Components;

using Toolbox.Extensions;
using Toolbox.Tools;

namespace GFSWeb.Application;

public static class NavHelper
{
    public static string AccessManagement => "/access";
    public static string UnderConstructionPath => "/underConstruction";

    //public static void Goto
    //public static void GotoCalendar(this NavigationManager nav) => nav.NotNull().NavigateTo(MarathonCalendarPath);

    public static string GetPrincipalHref(string id, string? returnUrl = null)
    {
        id = Uri.EscapeDataString(id);
        returnUrl = returnUrl?.Func(x => Uri.EscapeDataString(x));

        var url = Build([AccessManagement, id], BuildReturnUrl(returnUrl));
        return $"{NavHelper.AccessManagement}/{id}?returnUrl={returnUrl}";
    }

    public static void GotoPrincipalEditPage(this NavigationManager nav, string id, string? returnUrl = null)
    {
        var url = GetPrincipalHref(id, returnUrl);
        nav.NotNull().NavigateTo(url);
    }

    //public static void GotoMarathonReview(this NavigationManager nav, string id, string? returnUrl = null)
    //{
    //    var url = Build([MarathonReviewPath, id], BuildReturnUrl(returnUrl));
    //    nav.NotNull().NavigateTo(url);
    //}

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
