using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Models;

public record ReportPackageRecord
{
    public string PackageId { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string MenuId { get; set; } = null!;
    public string Data { get; set; } = null!;
    public bool Disabled { get; set; }
    public DateTime DateTimeStamp { get; set; }
    public string UserStamp { get; set; } = null!;

    public static IValidator<ReportPackageRecord> Validator { get; } = new Validator<ReportPackageRecord>()
        .RuleFor(x => x.PackageId).NotEmpty()
        .RuleFor(x => x.Description).NotEmpty()
        .RuleFor(x => x.MenuId).NotEmpty()
        .RuleFor(x => x.Data).NotEmpty()
        .Build();
}

public static class ElimOperationRecordExtensions
{
    public static Option Validate(this ReportPackageRecord record) => ReportPackageRecord.Validator.Validate(record).ToOptionStatus();
}
