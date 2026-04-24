using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Tools;

namespace Toolbox.test.Tools;

public class StorePathTool_RemoveForwardSlashTests
{
    [Fact]
    public void RemoveForwardSlash_PathWithLeadingSlash_RemovesSlash()
    {
        var result = StorePathTool.RemoveForwardSlash("/folder/file.json");
        result.Be("folder/file.json");
    }

    [Fact]
    public void RemoveForwardSlash_PathWithoutLeadingSlash_ReturnsUnchanged()
    {
        var result = StorePathTool.RemoveForwardSlash("folder/file.json");
        result.Be("folder/file.json");
    }

    [Fact]
    public void RemoveForwardSlash_SingleSlash_ReturnsEmpty()
    {
        var result = StorePathTool.RemoveForwardSlash("/");
        result.Be("");
    }
}
