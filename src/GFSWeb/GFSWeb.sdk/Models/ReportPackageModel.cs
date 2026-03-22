using Toolbox.Tools;

namespace GFSWeb.sdk.Models;

public enum PackageType
{
    None,
    Elimination,
    Recons,
    ElimTrueUp,
    SpecialFunctions,
    GLSUs,
    Reports,
    MoreReports,
    Tables,
    UserManuals
}

public class ReportPackageModel
{
    public string PackageId { get; set; } = null!;
    public string SortKey { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ParentPackageId { get; set; } = null!;
    public PackageType PackageType { get; set; }

    public EliminationRecord? Elimination { get; init; }
    public IReadOnlyList<ElimSelectRecord> ElimSelects { get; init; } = Array.Empty<ElimSelectRecord>();
    public IReadOnlyList<MiscTablesRecord> SqlCommand { get; init; } = Array.Empty<MiscTablesRecord>();
    public IReadOnlyList<MiscTablesRecord> SqlSelect { get; init; } = Array.Empty<MiscTablesRecord>();

    public static IValidator<ReportPackageModel> Validator { get; } = new Validator<ReportPackageModel>()
        .RuleFor(x => x.PackageId).NotEmpty()
        .RuleFor(x => x.SortKey).NotEmpty()
        .RuleFor(x => x.Description).NotEmpty()
        .RuleFor(x => x.ParentPackageId).NotEmpty()
        .RuleFor(x => x.PackageType).ValidEnum()
        .RuleFor(x => x.ElimSelects).NotNull()
        .RuleFor(x => x.SqlCommand).NotNull()
        .RuleFor(x => x.SqlSelect).NotNull()
        .Build();
}

public static class ReportPackageModelTool
{
    public static PackageType GetPackageType(string? packageType) => packageType switch
    {
        "E" => PackageType.Elimination,
        "F" => PackageType.Recons,
        "P" => PackageType.ElimTrueUp,
        "S" => PackageType.SpecialFunctions,
        "G" => PackageType.GLSUs,
        "R" => PackageType.Reports,
        "M" => PackageType.MoreReports,
        "W" => PackageType.Tables,
        "T" => PackageType.UserManuals,
        _ => PackageType.None
    };
}
