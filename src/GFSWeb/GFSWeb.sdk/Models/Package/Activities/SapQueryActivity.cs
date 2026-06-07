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