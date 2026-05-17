using Toolbox.Tools;

namespace GFSWeb.Components.Pages.Package.Edit;

public record SapQueryMapping
{
    public string FieldIndex { get; set; } = null!;
    public string RfcColumn { get; set; } = null!;
    public string OutColumn { get; set; } = null!;

    public static IValidator<SapQueryMapping> Validator { get; } = new Validator<SapQueryMapping>()
        .RuleFor(x => x.FieldIndex).NotEmpty()
        .RuleFor(x => x.RfcColumn).NotEmpty()
        .RuleFor(x => x.OutColumn).NotEmpty()
        .Build();
}