using Toolbox.Tools;
using Toolbox.Types;

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
    Writes,
    Tables,
    UserManuals
}

public record PipelinePackageRecord
{
    public string PackageId { get; init; } = null!;
    public string SortKey { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string MenuId { get; init; } = null!;
    public PackageType PackageType { get; init; }

    public EliminationRecord? Elimination { get; init; }
    public IReadOnlyList<ElimSelectRecord> ElimSelects { get; init; } = Array.Empty<ElimSelectRecord>();
    public IReadOnlyList<MiscTablesRecord> MiscTables { get; init; } = Array.Empty<MiscTablesRecord>();

    public IReadOnlyList<IPackageActivity> Activities { get; init; } = Array.Empty<IPackageActivity>();

    public static IValidator<PipelinePackageRecord> Validator { get; } = new Validator<PipelinePackageRecord>()
        .RuleFor(x => x.PackageId).NotEmpty()
        .RuleFor(x => x.SortKey).NotEmpty()
        .RuleFor(x => x.Description).NotEmpty()
        .RuleFor(x => x.MenuId).NotEmpty()
        .RuleFor(x => x.PackageType).ValidEnum()
        .RuleFor(x => x.ElimSelects).NotNull()
        .RuleFor(x => x.MiscTables).NotNull()
        .RuleFor(x => x.Activities).NotNull()
        .Build();
}

public static class PipelinePackageRecordTool
{
    public static Option Validate(this PipelinePackageRecord record) => PipelinePackageRecord.Validator.Validate(record).ToOptionStatus();

    public static PipelinePackageRecord Build(this PipelinePackageRecord subject)
    {
        var n = subject with
        {
            Activities = [
                    new SapQueryActivity
                    {
                        SapQueryMappings = BuildQueryMapping(subject),
                        SapQueries = BuildSelects(subject),
                    }
                ],
        };

        return n;
    }

    public static List<MiscTablesRecord> GetSection(this PipelinePackageRecord subject, SectionId sectionId) => subject.MiscTables
        .Where(x => x.GetSectionId() == sectionId)
        .ToList();

    private static List<SapQueryMapping> BuildQueryMapping(PipelinePackageRecord subject)
    {
        var fieldSelects = subject.GetSection(SectionId.FieldSelect).Select(x => (type: 0, x.Descr, Field1: x.Field1.NotEmpty())).ToArray();
        var outSelects = subject.GetSection(SectionId.OutSelect).Select(x => (type: 1, x.Descr, Field1: x.Field1.NotEmpty())).ToArray();
        fieldSelects.Length.Be(outSelects.Length);

        var result = fieldSelects
            .Concat(outSelects)
            .GroupBy(x => x.Field1, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var result2 = result
            .Select(x => new SapQueryMapping
            {
                FieldIndex = x.Key,
                RfcColumn = x.First(x => x.type == 0).Descr,
                OutColumn = x.First(x => x.type == 1).Descr,
            }).ToList();

        return result2;
    }

    private static List<WhereEditRecord> BuildSelects(PipelinePackageRecord subject)
    {
        var result = subject.ElimSelects
            .GroupBy(x => x.Pass)
            .Select(x => new WhereEditRecord
            {
                PassNumber = x.Key,
                FieldNames = ElimSelectRecordTool.FieldNames,
                WhereItems = whereClauses(x),
            }).ToList();

        return result;

        static List<WhereClause> whereClauses(IEnumerable<ElimSelectRecord> records)
        {
            var list = records.Select(x => new WhereClause
            {
                FieldName = x.FieldName,
                Operator = OperatorTool.ToOperator(x.Oper, x.IncExcl),
                Value = x.FromVal,
                Value2 = x.ThruVal,
            }).ToList();

            return list;
        }
    }
}
