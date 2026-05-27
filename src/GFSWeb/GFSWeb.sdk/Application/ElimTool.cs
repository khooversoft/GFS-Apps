using System;
using System.Collections.Generic;
using System.Text;
using GFSWeb.sdk.Models;

namespace GFSWeb.sdk.Application;

public static class ElimTool
{
    public static PackageType GetPackageType(string? packageType) => packageType switch
    {
        "E" => PackageType.Elimination,         // 0
        "F" => PackageType.Recons,              // 1
        "P" => PackageType.ElimTrueUp,          // 2
        "S" => PackageType.SpecialFunctions,    // 3
        "G" => PackageType.GLSUs,               // 4
        "R" => PackageType.Reports,             // 5
        "M" => PackageType.MoreReports,         // 6
        "W" => PackageType.Writes,              // 7
        "T" => PackageType.Tables,              // 8
        "K" => PackageType.UserManuals,         // 9
        _ => PackageType.None
    };
}
