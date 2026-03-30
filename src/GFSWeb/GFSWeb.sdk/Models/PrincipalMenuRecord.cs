using System;
using System.Collections.Generic;
using System.Text;
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
    public static bool Like(this PrincipalMenuRecord record, string search)
    {
        record.NotNull();
        if (search.IsEmpty()) return false;

        var realSearch = search switch
        {
            var s when s.IndexOf('*') < 0 || s.IndexOf('%') < 0 => record.MenuDescription.Contains(search) || record.Description.Contains(search),
            _ => record.MenuDescription.Like(search) || record.Description.Like(search)
        };

        return realSearch;
    }
}