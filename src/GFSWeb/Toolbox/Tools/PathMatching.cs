using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Toolbox.Tools;

/// <summary>
/// Pattern Meaning
/// 
/// *       Matches zero or more characters in a file or folder name(but not / or \).
/// ?       Matches exactly one character(except / or \).
/// **      Matches zero or more directory levels(recursive).
/// {a,b}	Matches either a or b (brace expansion).
/// [abc]   Matches any one of the characters a, b, or c.
/// [!abc]  Matches any character except a, b, or c.
/// 
/// This is case-insensitive on Windows but case-sensitive on Linux/macOS unless you normalize.
/// Brace expansion {} works for multiple extensions or names.
/// 
/// Examples:
/// 
/// one.txt             Eact match
/// dir/two.txt         Eact match
/// *.txt               Match all .txt files
/// ?.txt               Match any single-character .txt file name
/// file[1-3].txt       Match file1.txt, file2.txt, file3.txt
/// [!tT]emp*           Match all files except those starting with "temp"
/// *.{jpg,png}         Match either .jpg or .png files
/// *.txt	            All files with .txt file extension.
/// *.*                 All files with an extension.
/// *	                All files in top-level directory.
/// .*	                File names beginning with '.'.
/// *word*              All files with 'word' in the filename.
/// readme.*            All files named 'readme' with any file extension.
/// styles/*.css	    All files with extension '.css' in the directory 'styles/'.
/// scripts/*/*         All files in 'scripts/' or one level of subdirectory under 'scripts/'.
/// images*/*	        All files in a folder with name that is or begins with 'images'.
/// **/*	            All files in any subdirectory.
/// dir/**/*            All files in any subdirectory under 'dir/'.
/// dir/	            All files in any subdirectory under 'dir/'.
/// 
/// </summary>
public class PathMatching
{
    private static readonly Regex _multipleSlashesRegex = new Regex("/{2,}", RegexOptions.Compiled);
    private static readonly Regex _doubleStarRegex = new Regex(@"\*\*", RegexOptions.Compiled);
    private static readonly Regex _multipleStarsInDirectoriesRegex = new Regex(@"\*.*\*", RegexOptions.Compiled);
    private readonly Regex _regex;

    public PathMatching(string pattern)
    {
        pattern.NotEmpty();

        string normalized = NormalizePath(pattern);

        // Extract the static base path prefix (before any glob characters)
        int firstGlob = normalized.IndexOfAny(['*', '?', '[', '{']);
        if (firstGlob >= 0)
        {
            int lastSlash = normalized.LastIndexOf('/', firstGlob);
            BasePath = lastSlash >= 0 ? normalized[..lastSlash] : string.Empty;
        }
        else
        {
            BasePath = normalized;
        }

        IsRecursive = CalculateIsRecursive(normalized);

        // Convert glob pattern to a regex and anchor to ensure the entire path is matched
        _regex = new Regex(GlobToRegex(normalized), RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

    public string BasePath { get; }
    public bool IsRecursive { get; }

    public bool Match(string path)
    {
        string normalizedFileName = NormalizePath(path);
        return _regex.IsMatch(normalizedFileName);
    }

    private static bool CalculateIsRecursive(string normalizedPattern)
    {
        if (_doubleStarRegex.IsMatch(normalizedPattern))
            return true;

        int lastSlashIndex = normalizedPattern.LastIndexOf('/');
        string directoryPart = lastSlashIndex >= 0 ? normalizedPattern[..lastSlashIndex] : string.Empty;

        return _multipleStarsInDirectoriesRegex.IsMatch(directoryPart);
    }

    private static string GlobToRegex(string pattern)
    {
        var sb = new StringBuilder("^");
        int i = 0;

        while (i < pattern.Length)
        {
            char c = pattern[i];

            if (c == '*' && i + 1 < pattern.Length && pattern[i + 1] == '*')
            {
                // **/ → optional recursive path prefix; ** alone → match everything remaining
                if (i + 2 < pattern.Length && pattern[i + 2] == '/')
                {
                    sb.Append("(?:.*/)?");
                    i += 3;
                }
                else
                {
                    sb.Append(".*");
                    i += 2;
                }
            }
            else if (c == '*')
            {
                sb.Append("[^/]*");
                i++;
            }
            else if (c == '?')
            {
                sb.Append("[^/]");
                i++;
            }
            else if (c == '[')
            {
                // Convert [! negation to [^ for regex character classes
                i++;
                if (i < pattern.Length && pattern[i] == '!')
                {
                    sb.Append("[^");
                    i++;
                }
                else
                {
                    sb.Append('[');
                }

                while (i < pattern.Length && pattern[i] != ']') sb.Append(pattern[i++]);

                if (i < pattern.Length)
                {
                    sb.Append(']');
                    i++;
                }
            }
            else if (c == '{')
            {
                // Brace expansion {a,b} → (?:a|b)
                int closeIndex = pattern.IndexOf('}', i + 1);

                if (closeIndex >= 0)
                {
                    string[] parts = pattern[(i + 1)..closeIndex].Split(',');
                    sb.Append("(?:");
                    for (int k = 0; k < parts.Length; k++)
                    {
                        if (k > 0) sb.Append('|');
                        sb.Append(Regex.Escape(parts[k]));
                    }
                    sb.Append(')');
                    i = closeIndex + 1;
                }
                else
                {
                    sb.Append(Regex.Escape("{"));
                    i++;
                }
            }
            else
            {
                sb.Append(Regex.Escape(c.ToString()));
                i++;
            }
        }

        sb.Append('$');
        return sb.ToString();
    }

    private static string NormalizePath(string path)
    {
        string normalized = path.NotEmpty().Replace('\\', '/');
        return _multipleSlashesRegex.Replace(normalized, "/");
    }
}

