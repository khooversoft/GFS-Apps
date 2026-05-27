//using Toolbox.Tools;

//namespace GFSWeb.sdk.Models;

//public record ReportPackageModel
//{
//    public string PackageId { get; set; } = null!;
//    public string SortKey { get; set; } = null!;
//    public string Description { get; set; } = null!;
//    public string MenuId { get; set; } = null!;
//    public PackageType PackageType { get; set; }

//    public EliminationRecord? Elimination { get; init; }
//    public IReadOnlyList<ElimSelectRecord> ElimSelects { get; init; } = Array.Empty<ElimSelectRecord>();
//    public IReadOnlyList<MiscTablesRecord> MiscTables { get; init; } = Array.Empty<MiscTablesRecord>();

//    public IReadOnlyList<IPackageActivity> Packages { get; init; } = Array.Empty<IPackageActivity>();

//    public static IValidator<ReportPackageModel> Validator { get; } = new Validator<ReportPackageModel>()
//        .RuleFor(x => x.PackageId).NotEmpty()
//        .RuleFor(x => x.SortKey).NotEmpty()
//        .RuleFor(x => x.Description).NotEmpty()
//        .RuleFor(x => x.MenuId).NotEmpty()
//        .RuleFor(x => x.PackageType).ValidEnum()
//        .RuleFor(x => x.ElimSelects).NotNull()
//        .RuleFor(x => x.MiscTables).NotNull()
//        .RuleFor(x => x.Packages).NotNull()
//        .Build();
//}
