using Toolbox.Tools;

namespace Toolbox.Test.Tools;

public class PathMatchingTests
{
    [Theory]
    // Exact path
    [InlineData("one.txt", "one.txt", true)]
    [InlineData("dir/two.txt", "dir/two.txt", true)]
    [InlineData("other.txt", "one.txt", false)]
    [InlineData("subdir/one.txt", "one.txt", false)]

    // * wildcard - zero or more chars (not /)
    [InlineData("notes.txt", "*.txt", true)]
    [InlineData("readme.md", "*.txt", false)]
    [InlineData("dir/notes.txt", "*.txt", false)]
    [InlineData("file.js", "*", true)]
    [InlineData("dir/file.js", "*", false)]
    [InlineData("file.log", "*.*", true)]
    [InlineData("noextension", "*.*", false)]
    [InlineData(".hidden", ".*", true)]
    [InlineData("visible.txt", ".*", false)]
    [InlineData("error-log.txt", "*log*", true)]
    [InlineData("error-txt.txt", "*log*", false)]
    [InlineData("readme.txt", "readme.*", true)]
    [InlineData("readme.md", "readme.*", true)]
    [InlineData("readout.txt", "readme.*", false)]
    [InlineData("styles/main.css", "styles/*.css", true)]
    [InlineData("styles/sub/main.css", "styles/*.css", false)]
    [InlineData("scripts/js/app.js", "scripts/*/*", true)]
    [InlineData("scripts/app.js", "scripts/*/*", false)]
    [InlineData("images/file.txt", "images*/*", true)]
    [InlineData("images2/file.txt", "images*/*", true)]
    [InlineData("docs/file.txt", "images*/*", false)]

    // ? wildcard - exactly one char (not /)
    [InlineData("a.txt", "?.txt", true)]
    [InlineData("ab.txt", "?.txt", false)]

    // [abc] / [1-3] character class
    [InlineData("file1.txt", "file[1-3].txt", true)]
    [InlineData("file2.txt", "file[1-3].txt", true)]
    [InlineData("file3.txt", "file[1-3].txt", true)]
    [InlineData("file4.txt", "file[1-3].txt", false)]
    [InlineData("fileA.txt", "file[abc].txt", true)]
    [InlineData("fileD.txt", "file[abc].txt", false)]

    // [!abc] negated character class
    [InlineData("temp-file.txt", "[!tT]emp*", false)]
    [InlineData("Temp-file.txt", "[!tT]emp*", false)]
    [InlineData("xemp-file.txt", "[!tT]emp*", true)]

    // {a,b} brace expansion
    [InlineData("photo.jpg", "*.{jpg,png}", true)]
    [InlineData("photo.png", "*.{jpg,png}", true)]
    [InlineData("photo.gif", "*.{jpg,png}", false)]
    [InlineData("report.txt", "*.{txt,md,log}", true)]
    [InlineData("report.md", "*.{txt,md,log}", true)]
    [InlineData("report.csv", "*.{txt,md,log}", false)]

    // ** recursive matching
    [InlineData("dir/sub/file.txt", "dir/**/file.txt", true)]
    [InlineData("dir/sub/sub2/file.txt", "dir/**/file.txt", true)]
    [InlineData("dir/file.txt", "dir/**/file.txt", true)]
    [InlineData("other/sub/file.txt", "dir/**/file.txt", false)]
    [InlineData("file.txt", "**/*", true)]
    [InlineData("dir/file.txt", "**/*", true)]
    [InlineData("dir/file.txt", "**", true)]
    [InlineData("dir/file.txt", "dir/**", true)]
    [InlineData("dir/sub/file.txt", "**/*", true)]
    [InlineData("dir/file.txt", "dir/**/*", true)]
    [InlineData("dir/sub/file.txt", "dir/**/*", true)]
    [InlineData("other/file.txt", "dir/**/*", false)]
    [InlineData("nodes/path/path1/file.json", "**", true)]
    public void Match_Patterns(string path, string pattern, bool expected)
    {
        var subject = new PathMatching(pattern);
        subject.Match(path).Be(expected);
    }

    [Theory]
    [InlineData("dir\\sub\\file.txt", "dir/**/file.txt", true)]
    [InlineData("path//double//slash.txt", "path/double/slash.txt", true)]
    public void Match_PathNormalization(string path, string pattern, bool expected)
    {
        var subject = new PathMatching(pattern);
        subject.Match(path).Be(expected);
    }

    [Theory]
    [InlineData("one.txt", "one.txt")]
    [InlineData("dir/two.txt", "dir/two.txt")]
    [InlineData("*.txt", "")]
    [InlineData("dir/*.txt", "dir")]
    [InlineData("dir/**/file.txt", "dir")]
    [InlineData("folder/{a,b}/file.txt", "folder")]
    [InlineData("folder/file[1-3].txt", "folder")]
    [InlineData("folder/folder2/file[1-3].txt", "folder/folder2")]
    [InlineData("**/*", "")]
    public void BasePath_FromPattern(string pattern, string expectedBasePath)
    {
        var subject = new PathMatching(pattern);
        subject.BasePath.Be(expectedBasePath);
    }

    [Theory]
    [InlineData("one.txt", false)]
    [InlineData("dir/*.txt", false)]
    [InlineData("images*/*", false)]
    [InlineData("dir/**/file.txt", true)]
    [InlineData("**/*", true)]
    [InlineData("**", true)]
    [InlineData("***", true)]
    [InlineData("folder/*/*/file.txt", true)]
    public void IsRecursive_FromPattern(string pattern, bool expected)
    {
        var subject = new PathMatching(pattern);
        subject.IsRecursive.Be(expected);
    }
}
