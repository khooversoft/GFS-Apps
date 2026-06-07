using GFSWeb.sdk.Models;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Services;

public class ActivityInfoRecord
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public object? Source { get; set; }

    public static IValidator<ActivityInfoRecord> Validator => new Validator<ActivityInfoRecord>()
        .RuleFor(x => x.Id).NotEmpty()
        .RuleFor(x => x.Type).NotEmpty()
        .RuleFor(x => x.Description).NotEmpty()
        .RuleFor(x => x.Icon).NotEmpty()
        .Build();
}

public static class ActivityInfoRecordTool
{
    public static Option Validate(this ActivityInfoRecord record) => ActivityInfoRecord.Validator.Validate(record).ToOptionStatus();
}
