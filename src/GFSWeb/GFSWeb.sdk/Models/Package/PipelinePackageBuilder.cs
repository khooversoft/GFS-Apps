using Toolbox.Extensions;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Models;

public class PipelinePackageBuilder
{
    public string PackageId { get; set; } = null!;
    public string SortKey { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string MenuId { get; set; } = null!;
    public PackageType PackageType { get; set; }

    public EliminationRecord? Elimination { get; set; }
    public List<ElimSelectRecord> ElimSelects { get; set; } = new();
    public List<MiscTablesRecord> MiscTables { get; set; } = new();
    public List<KeyValue<string>> Properties { get; init; } = new();

    public PipelinePackageRecord Build()
    {
        var n = new PipelinePackageRecord
        {
            PackageId = PackageId.NotEmpty(),
            SortKey = SortKey.NotEmpty(),
            Description = Description.NotEmpty(),
            MenuId = MenuId.NotEmpty(),
            PackageType = PackageType.Assert(x => x.IsEnumValid(), "Invalid PackageType"),
            Elimination = Elimination,
            ElimSelects = ElimSelects.ToArray(),
            MiscTables = MiscTables.ToArray(),
            Properties = Properties.ToArray(),
            Activities = [
                new SapQueryActivity
                {
                    Description = "Import from V1",
                    SapQueryMappings = BuildQueryMapping(),
                    SapQueries = BuildSelects(),
                },
                .. BuildSqlCommand()
            ]
        };

        return n;
    }

    private List<MiscTablesRecord> GetSection(SectionId sectionId) => MiscTables
        .Where(x => x.GetSectionId() == sectionId)
        .ToList();

    private List<SapQueryMapping> BuildQueryMapping()
    {
        var fieldSelects = GetSection(SectionId.FieldSelect).Select(x => (type: 0, x.Descr, Field1: x.Field1.NotEmpty())).ToArray();
        var outSelects = GetSection(SectionId.OutSelect).Select(x => (type: 1, x.Descr, Field1: x.Field1.NotEmpty())).ToArray();

        if (fieldSelects.Length == 0 || outSelects.Length == 0) return [];

        var result = fieldSelects
            .Concat(outSelects)
            .GroupBy(x => x.Field1, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var result2 = result
            .Select(x => new SapQueryMapping
            {
                FieldIndex = x.Key,
                RfcColumn = x.OrderBy(x => x.type).First().Descr,
                OutColumn = x.OrderByDescending(x => x.type).First().Descr,
            }).ToList();

        return result2;
    }

    private List<WhereEditRecord> BuildSelects()
    {
        var result = ElimSelects
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

    private IReadOnlyList<SqlCommandActivity> BuildSqlCommand()
    {
        var list = GetSection(SectionId.Sql);

        var result = list.Select(x => new SqlCommandActivity
        {
            Id = x.ID,
            SqlCommand = x.Descr,
            Description = "Import from V1",
        }.WithHash()
        ).ToArray();

        return result;
    }
}
