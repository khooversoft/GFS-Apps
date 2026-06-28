using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Models;

public record SapQueryActivity : IPackageActivity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Type { get; } = "SAP Query";
    public string Description { get; set; } = null!;

    public List<SapQueryMapping> SapQueryMappings { get; init; } = new();
    public List<WhereEditRecord> SapQueries { get; init; } = new();

    public bool Search(string searchTerm)
    {
        return Id.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
               Type.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
               Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
               SapQueryMappings.Any(x => x.Search(searchTerm)) ||
               SapQueries.Any(x => x.Search(searchTerm));
    }

    public static IValidator<SapQueryActivity> Validator { get; } = new Validator<SapQueryActivity>()
        .RuleFor(x => x.Id).NotEmpty()
        .RuleFor(x => x.Description).NotEmpty()
        .RuleFor(x => x.SapQueryMappings).NotNull()
        .RuleFor(x => x.SapQueries).NotNull()
        .Build();
}

public static class SapQueryActivityTool
{
    public static Option Validate(this SapQueryActivity subject) => SapQueryActivity.Validator.Validate(subject).ToOptionStatus();
}