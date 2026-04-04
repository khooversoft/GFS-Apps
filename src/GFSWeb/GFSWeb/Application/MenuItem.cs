using Toolbox.Extensions;
using Toolbox.Tools;

namespace GFSWeb.Application;

public record MenuItem
{
    public MenuItem() { }

    public MenuItem(string key, string value)
    {
        Key = key.NotEmpty();
        Value = value.NotEmpty();
    }

    public string Key { get; init; } = null!;
    public string Value { get; init; } = null!;
}

public static class MenuItemExtensions
{
    public static bool Like(this MenuItem subject, string? search)
    {
        subject.NotNull();
        if (search.IsEmpty()) return true;

        var realSearch = search switch
        {
            var s when s.IndexOf('*') < 0 || s.IndexOf('%') < 0 => subject.Key.Contains(search, StringComparison.OrdinalIgnoreCase),
            _ => subject.Value.Like(search)
        };

        return realSearch;
    }
}
