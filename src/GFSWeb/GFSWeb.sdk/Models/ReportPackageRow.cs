using Toolbox.Extensions;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Models;

public record ReportPackageRow
{
    public string PackageId { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string MenuId { get; set; } = null!;
    public string Data { get; set; } = null!;
    public bool Disabled { get; set; }
    public DateTime DateTimeStamp { get; set; }
    public string UserStamp { get; set; } = null!;

    public static IValidator<ReportPackageRow> Validator { get; } = new Validator<ReportPackageRow>()
        .RuleFor(x => x.PackageId).NotEmpty()
        .RuleFor(x => x.Description).NotEmpty()
        .RuleFor(x => x.MenuId).NotEmpty()
        .RuleFor(x => x.Data).NotEmpty()
        .Build();
}

public static class ElimOperationRecordExtensions
{
    public static Option Validate(this ReportPackageRow record) => ReportPackageRow.Validator.Validate(record).ToOptionStatus();

    public static PipelinePackageRecord Unpack(this ReportPackageRow record)
    {
        record.Validate().ThrowOnError();
        return record.Data.ToObject<PipelinePackageRecord>();
    }
}
