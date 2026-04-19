using Toolbox.Extensions;
using Toolbox.Tools;

namespace GFSWeb.sdk.Models;

public record PrincipalMenuRecord
{
    public string MenuId { get; init; } = null!;
    public string MenuDescription { get; init; } = null!;
    public string PackageId { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string Data { get; init; } = null!;
}


public static class PrincipalMenuRecordExtensions
{
    public static bool Like(this PrincipalMenuRecord record, string? search)
    {
        record.NotNull();
        if (search.IsEmpty()) return true;

        var realSearch = search switch
        {
            var s when s.IndexOf('*') < 0 || s.IndexOf('%') < 0 =>
                record.MenuDescription.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                record.Description.Contains(search, StringComparison.OrdinalIgnoreCase),

            _ => record.MenuDescription.Like(search) || record.Description.Like(search)
        };

        return realSearch;
    }

    public static bool LikeDescription(this PrincipalMenuRecord record, string? search)
    {
        record.NotNull();
        if (search.IsEmpty()) return true;

        var realSearch = search switch
        {
            var s when s.IndexOf('*') < 0 || s.IndexOf('%') < 0 => record.Description.Contains(search, StringComparison.OrdinalIgnoreCase),
            _ => record.Description.Like(search)
        };

        return realSearch;
    }
}