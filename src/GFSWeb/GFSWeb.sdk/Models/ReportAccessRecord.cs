using System.Collections.Frozen;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Models;

public static class ReportAccessRole
{
    public const string ReaderRole = "reader";
    public const string ContributorRole = "contributor";
    public const string OwnerRole = "owner";

    public static FrozenSet<string> ValidAccess { get; } = new string[]
    {
        ReportAccessRole.ReaderRole,
        ReportAccessRole.ContributorRole,
        ReportAccessRole.OwnerRole
    }
    .ToFrozenSet();
}

public record ReportAccessRecord
{
    public string PackageId { get; init; } = null!;
    public string NameIdentifier { get; init; } = null!;
    public string Access { get; init; } = null!;

    public static IValidator<ReportAccessRecord> Validator { get; } = new Validator<ReportAccessRecord>()
        .RuleFor(x => x.PackageId).NotEmpty()
        .RuleFor(x => x.NameIdentifier).NotEmpty()
        .RuleFor(x => x.Access).NotEmpty().Must(x => ReportAccessRole.ValidAccess.Contains(x) ? StatusCode.OK : StatusCode.NotFound)
        .Build();
}


public static class ReportAccessRecordExtensions
{
    public static Option Validate(this ReportAccessRecord record) => ReportAccessRecord.Validator.Validate(record).ToOptionStatus();
}