using System;
using System.Collections.Generic;
using System.Text;

namespace GFSWeb.sdk.Models;

public record GroupPackageAccessRecord
{
    public string PackageId { get; init; } = null!;
    public string GroupName { get; init; } = null!;
    public string Access { get; init; } = null!;
}
