using Toolbox.Extensions;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Models;

public record MenuRecord
{
    public string MenuId { get; init; } = null!;
    public string Description { get; init; } = null!;

    public static IValidator<MenuRecord> Validator { get; } = new Validator<MenuRecord>()
        .RuleFor(x => x.MenuId).NotEmpty()
        .RuleFor(x => x.Description).NotEmpty()
        .Build();
}

public static class MenuRecordTool
{
    public static Option Validate(this MenuRecord subject) => MenuRecord.Validator.Validate(subject).ToOptionStatus();

    public static MenuRecord ConvertTo(this ElimTreeRecord subject) => new MenuRecord
    {
        MenuId = subject.Id,
        Description = subject.Descr,
    };
}