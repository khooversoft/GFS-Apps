using Toolbox.Extensions;

namespace Toolbox.Tools;

public static class StorePathTool
{
    public static string RemoveForwardSlash(string path) => path.NotEmpty().StartsWith('/') switch
    {
        true => path[1..],
        false => path,
    };

    public static string ToFolderSearch(string path, bool recursive = false) => recursive switch
    {
        false => GetRootPath(path) + "/*",
        true => GetRootPath(path) + "/**",
    };

    public static string AddRecursiveSafe(string path) => path.NotEmpty() switch
    {
        "**" => path,
        "***" => "**",
        string _ when path.IsEmpty() => "**",
        string _ when path.EndsWith("/**") => path,
        string _ when path.EndsWith("/*") => path + "*",
        _ => path + "/**",
    };

    /// <summary>
    /// Gets the normalized root path for a store path and appends any additional path segments.
    /// </summary>
    /// <param name="path">
    /// The source path. Wildcard suffixes such as <c>/*</c> and <c>/**</c> are removed before the root path is built.
    /// </param>
    /// <param name="additionalPaths">
    /// Optional additional path values to append. Each value can contain one or more <c>/</c>-delimited segments.
    /// </param>
    /// <returns>
    /// A lower-case path composed from the root portion of <paramref name="path"/> and the appended path segments.
    /// </returns>
    public static string GetRootPath(string path, params string[] additionalPaths)
    {
        path.NotEmpty();
        int idx = path.IndexOf('*');

        var rootPath = idx switch
        {
            -1 => path,
            int v => path[..v].Func(x =>
            {
                int lastSlashIdx = x.LastIndexOf('/');
                return lastSlashIdx switch
                {
                    -1 => string.Empty,
                    var v when v == x.Length - 1 => x[..^1],
                    _ => x[..lastSlashIdx],
                };
            })
        };

        var addParts = additionalPaths.SelectMany(x => x.Split('/', StringSplitOptions.RemoveEmptyEntries));

        var fullPath = rootPath
            .Split('/', StringSplitOptions.RemoveEmptyEntries)
            .Concat(addParts)
            .Join('/');

        return fullPath.ToLowerInvariant();
    }

    /// <summary>
    /// Gets the file name from a store path, preserving the file extension.
    /// </summary>
    /// <param name="path">The full store path.</param>
    /// <returns>The last path segment, including its extension when present.</returns>
    public static string GetFileName(string path)
    {
        path.NotEmpty();

        var span = path.AsSpan().TrimEnd('/');

        if (span.IsEmpty) return string.Empty;

        int lastSlash = span.LastIndexOf('/');
        return lastSlash switch
        {
            -1 => span.ToString(),
            _ => span[(lastSlash + 1)..].ToString(),
        };
    }

    public static string ToSafePath(string path, string? extension = null)
    {
        path.NotEmpty();

        // Ensure extension starts with '.'
        if (extension is not null)
        {
            if (!extension.StartsWith('.')) extension = "." + extension;

            // Remove existing extension from original path before converting to safe chars
            int lastSlash = path.LastIndexOf('/');
            int lastDot = path.LastIndexOf('.');

            if (lastDot > lastSlash && lastDot > 0) path = path[..lastDot];
        }

        var safeChars = path
            .Select(c => c switch
            {
                >= 'a' and <= 'z' => c,
                >= 'A' and <= 'Z' => char.ToLowerInvariant(c),
                >= '0' and <= '9' => c,
                '-' or '_' or '/' or '@' or '.' => c,
                _ => '_'
            })
            .ToArray();

        var result = new string(safeChars);

        return extension switch
        {
            null => result.ToLowerInvariant(),
            _ => result + extension.ToLowerInvariant()
        };
    }
}