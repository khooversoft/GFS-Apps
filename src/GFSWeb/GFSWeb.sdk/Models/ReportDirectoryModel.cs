using System;
using System.Collections.Generic;
using System.Text;

namespace GFSWeb.sdk.Models;

public class ReportDirectoryModel
{
    public IReadOnlyList<ElimTreeRecord> Items { get; init; } = Array.Empty<ElimTreeRecord>();
}
