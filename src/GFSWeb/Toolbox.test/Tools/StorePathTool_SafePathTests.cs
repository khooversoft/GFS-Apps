using Toolbox.Tools;

namespace Toolbox.test.Tools;

public class StorePathTool_SafePathTests
{
    [Fact]
    public void ToSafePath_ValidPath_ReturnsLowercase()
    {
        var result = StorePathTool.ToSafePath("Folder/File", ".json");
        result.Be("folder/file.json");
    }

    [Fact]
    public void ToSafePath_PathWithSpaces_ReplacesWithUnderscore()
    {
        var result = StorePathTool.ToSafePath("my file name", ".json");
        result.Be("my_file_name.json");
    }

    [Fact]
    public void ToSafePath_PathWithSpecialCharacters_ReplacesWithUnderscore()
    {
        var result = StorePathTool.ToSafePath("test(123#file$name", ".json");
        result.Be("test_123_file_name.json");
    }

    [Fact]
    public void ToSafePath_PathWithExistingExtension_ReplacesExtension()
    {
        var result = StorePathTool.ToSafePath("folder/file.txt", ".json");
        result.Be("folder/file.json");
    }

    [Fact]
    public void ToSafePath_ExtensionWithoutDot_AddsDot()
    {
        var result = StorePathTool.ToSafePath("folder/file", "json");
        result.Be("folder/file.json");
    }

    [Fact]
    public void ToSafePath_ExtensionWithDot_KeepsDot()
    {
        var result = StorePathTool.ToSafePath("folder/file", ".json");
        result.Be("folder/file.json");
    }

    [Fact]
    public void ToSafePath_UppercaseExtension_ReturnsLowercase()
    {
        var result = StorePathTool.ToSafePath("folder/file", ".JSON");
        result.Be("folder/file.json");
    }

    [Fact]
    public void ToSafePath_PathWithNumbers_PreservesNumbers()
    {
        var result = StorePathTool.ToSafePath("file123/test456", ".json");
        result.Be("file123/test456.json");
    }

    [Fact]
    public void ToSafePath_PathWithHyphensAndUnderscores_PreservesThem()
    {
        var result = StorePathTool.ToSafePath("my-file_name/test-path_here", ".json");
        result.Be("my-file_name/test-path_here.json");
    }

    [Fact]
    public void ToSafePath_PathWithSlashes_PreservesSlashes()
    {
        var result = StorePathTool.ToSafePath("folder/subfolder/file", ".json");
        result.Be("folder/subfolder/file.json");
    }

    [Fact]
    public void ToSafePath_PathWithDotInFolder_HandlesCorrectly()
    {
        var result = StorePathTool.ToSafePath("folder.name/file.txt", ".json");
        result.Be("folder_name/file.json");
    }

    [Fact]
    public void ToSafePath_PathWithMultipleDots_RemovesLastExtension()
    {
        var result = StorePathTool.ToSafePath("file.backup.old", ".json");
        result.Be("file_backup.json");
    }

    [Fact]
    public void ToSafePath_ComplexPath_HandlesAllRules()
    {
        var result = StorePathTool.ToSafePath("Users/John Doe/My Documents/Report #1.txt", ".json");
        result.Be("users/john_doe/my_documents/report__1.json");
    }
}
