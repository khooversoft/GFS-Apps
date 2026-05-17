using System.Collections.Frozen;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Models;

public record GroupPackageAccessRecord
{
    public string PackageId { get; init; } = null!;
    public string GroupName { get; init; } = null!;
    public string Role { get; init; } = null!;

    public static IValidator<GroupPackageAccessRecord> Validator { get; } = new Validator<GroupPackageAccessRecord>()
        .RuleFor(x => x.PackageId).NotEmpty()
        .RuleFor(x => x.GroupName).NotEmpty()
        .RuleFor(x => x.Role).NotEmpty().Must(x => GroupPackageAccessRecordTool.ValidRoles.Contains(x), x => $"{x} not valid")
        .Build();
}

public static class GroupPackageAccessRecordTool
{
    public static FrozenSet<string> ValidRoles { get; } = new string[] { "reader", "contributor", "owner" }.ToFrozenSet();

    public static Option Validate(this GroupPackageAccessRecord subject) => GroupPackageAccessRecord.Validator.Validate(subject).ToOptionStatus();
}
