using System;
using System.Collections.Generic;
using System.Text;

namespace GFSWeb.sdk.Models;

public class ReportDirectoryModel
{
    public IReadOnlyList<MenuRecord> Items { get; init; } = Array.Empty<MenuRecord>();
    public IReadOnlyList<PrincipalIdentityRecord> Users { get; init; } = Array.Empty<PrincipalIdentityRecord>();
    public IReadOnlyList<UserAccessRecord> UserAccess { get; init; } = Array.Empty<UserAccessRecord>();
}
