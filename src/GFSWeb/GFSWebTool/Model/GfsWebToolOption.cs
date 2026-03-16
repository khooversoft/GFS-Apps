using System;
using System.Collections.Generic;
using System.Text;

namespace GFSWebTool.Model;

internal class GfsWebToolOption
{
    public string OriginalConnectionString { get; init; } = null!;
    public string ManagementConnectionString { get; init; } = null!;

}
