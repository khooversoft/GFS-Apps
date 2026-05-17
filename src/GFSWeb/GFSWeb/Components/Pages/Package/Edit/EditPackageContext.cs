using System.Collections.Concurrent;
using GFSWeb.Application.Models;
using GFSWeb.sdk.Models;
using Toolbox.Extensions;
using Toolbox.Tools;

namespace GFSWeb.Components.Pages.Package.Edit;

public partial record EditPackageContext
{
    public string PackageId { get; set; } = null!;
    public string Description { get; set; } = null!;
    public PackageType PackageType { get; set; }

    public EliminationRecord? Elimination { get; init; }
    public IDictionary<string, ElimSelectRecord> ElimSelects { get; init; } = new ConcurrentDictionary<string, ElimSelectRecord>(StringComparer.OrdinalIgnoreCase);
    public IDictionary<string, MiscTablesRecord> MiscTables { get; init; } = new ConcurrentDictionary<string, MiscTablesRecord>(StringComparer.OrdinalIgnoreCase);

    public IDictionary<int, WhereEditContext> SapQueries { get; init; } = new ConcurrentDictionary<int, WhereEditContext>();
    public IDictionary<string, SapQueryMapping> SapQueryMappings { get; init; } = new ConcurrentDictionary<string, SapQueryMapping>(StringComparer.OrdinalIgnoreCase);

    public virtual bool Equals(EditPackageContext? context) => context is not null &&
        PackageId == context.PackageId &&
        Description == context.Description &&
        PackageType == context.PackageType &&
        Elimination == context.Elimination &&
        ElimSelects.DictionaryEquals(context.ElimSelects) &&
        MiscTables.DictionaryEquals(context.MiscTables);

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(EqualityContract);
        hash.Add(PackageId);
        hash.Add(Description);
        hash.Add(PackageType);
        hash.Add(Elimination);
        hash.Add(ElimSelects);
        hash.Add(MiscTables);
        return hash.ToHashCode();
    }
}

public static class EditPackageContextTool
{
    public static EditPackageContext ConvertTo(this ReportPackageModel subject)
    {
        subject.NotNull();

        var result = new EditPackageContext
        {
            PackageId = subject.PackageId,
            Description = subject.Description,
            PackageType = subject.PackageType,
            Elimination = subject.Elimination,
            ElimSelects = subject.ElimSelects.ToDictionary(_ => Guid.NewGuid().ToString(), x => x with { }),
            MiscTables = subject.MiscTables.ToDictionary(_ => Guid.NewGuid().ToString(), x => x with { }),
        };

        var newContext = result with
        {
            SapQueries = buildSelects(result),
        };

        return newContext;

        static IDictionary<int, WhereEditContext> buildSelects(EditPackageContext subject)
        {
            var result = subject.ElimSelects.Values
                .GroupBy(x => x.Pass)
                .Select(x => new WhereEditContext
                {
                    Pass = x.Key,
                    FieldNames = ElimSelectRecordTool.FieldNames,
                    WhereItems = whereClauses(x),
                })
                .ToDictionary(x => x.Pass, x => x);

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

    public static List<MiscTablesRecord> GetSection(this EditPackageContext subject, SectionId sectionId) => subject.MiscTables.Values.Where(x => x.GetSectionId() == sectionId).ToList();
}