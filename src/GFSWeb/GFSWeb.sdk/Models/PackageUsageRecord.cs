using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Models;

public record class PackageUsageRecord
{
    public string NameIdentifier { get; init; } = null!;
    public string PackageId { get; init; } = null!;
    public bool Favorite { get; init; }
    public DateTime? LastAccessed { get; init; }

    public static IValidator<PackageUsageRecord> Validator { get; } = new Validator<PackageUsageRecord>()
        .RuleFor(x => x.NameIdentifier).NotEmpty()
        .RuleFor(x => x.PackageId).NotEmpty()
        .Build();
}


public static class PackageUsageRecordTool
{
    public static Option Validate(this PackageUsageRecord record) => PackageUsageRecord.Validator.Validate(record).ToOptionStatus();
}