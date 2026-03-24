using System;
using System.Collections.Generic;
using System.Text;

namespace GFSWeb.sdk.Models;

public class ReportDirectoryModel
{
    public IReadOnlyList<MenuRecord> Items { get; init; } = Array.Empty<MenuRecord>();
}
