using Toolbox.Tools;

namespace GFSWeb.sdk.Models;

public record SapQueryMapping
{
    public string FieldIndex { get; set; } = null!;
    public string RfcColumn { get; set; } = null!;
    public string OutColumn { get; set; } = null!;

    public bool Search(string searchTerm)
    {
        return FieldIndex.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
               RfcColumn.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
               OutColumn.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
    }

    public static IValidator<SapQueryMapping> Validator { get; } = new Validator<SapQueryMapping>()
        .RuleFor(x => x.FieldIndex).NotEmpty()
        .RuleFor(x => x.RfcColumn).NotEmpty()
        .RuleFor(x => x.OutColumn).NotEmpty()
        .Build();
}