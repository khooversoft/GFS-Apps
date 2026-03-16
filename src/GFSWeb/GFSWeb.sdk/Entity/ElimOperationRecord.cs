using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Entity;

public record ElimOperationRecord
{
    public string ElimCode { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? Data { get; set; }
    public bool Disabled { get; set; }
    public DateTime DateTimeStamp { get; set; }
    public string UserStamp { get; set; } = null!;

    public static IValidator<ElimOperationRecord> Validator { get; } = new Validator<ElimOperationRecord>()
        .RuleFor(x => x.ElimCode).NotEmpty()
        .RuleFor(x => x.Description).NotEmpty()
        .RuleFor(x => x.UserStamp).NotEmpty()
        .Build();
}

public static class ElimOperationRecordExtensions
{
    public static Option Validate(this ElimOperationRecord record) => ElimOperationRecord.Validator.Validate(record).ToOptionStatus();
}
