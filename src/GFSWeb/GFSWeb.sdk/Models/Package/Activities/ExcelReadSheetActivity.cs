using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Models;

public class ExcelReadSheetActivity : IPackageActivity
{
    public string Id { get; set; } = null!;
    public string Type { get; } = "Read Excel";
    public string Description { get; set; } = null!;
    public string SpreadSheetPath { get; set; } = null!;

    public bool Search(string searchTerm)
    {
        return Id.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
               Type.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
               Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
               SpreadSheetPath.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
    }

    public static IValidator<ExcelReadSheetActivity> Validator { get; } = new Validator<ExcelReadSheetActivity>()
        .RuleFor(x => x.Id).NotEmpty()
        .RuleFor(x => x.Description).NotEmpty()
        .RuleFor(x => x.SpreadSheetPath).NotEmpty()
        .Build();
}

public static class ExcelReadSheetActivityTool
{
    public static Option Validate(this ExcelReadSheetActivity subject) => ExcelReadSheetActivity.Validator.Validate(subject).ToOptionStatus();
}