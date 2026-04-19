using System.Collections.Frozen;
using GFSWeb.sdk.Models;
using Toolbox.Tools;

namespace GFSWebTool.Commands;

public static class TableIdMap
{
    private readonly static FrozenSet<string> _requiredTableIds = new string[]
    {
        "GLSUs",
        "ReconExcelHdrs1",
        "ReconExcelHdrs2",
        "ReconExcelOrderBy",
        "ReconExcelComma",
        "ReconExcelEndColm",
        "ReconExcelPivot",
        "Pivot1Name",
        "Pivot1Rows",
        "Pivot1Colms",
        "Pivot1Page",
        "Pivot1Sum",
        "Pivot1Tabular",
        "Pivot1NoTotals",
        "FieldSelect",
        "FieldSelect",
        "FieldSelect",
        "FieldSelect",
        "FieldSelect",
        "FieldSelect",
        "FieldSelect",
        "FieldSelect",
        "OutSelect",
        "OutSelect",
        "OutSelect",
        "OutSelect",
        "OutSelect",
        "OutSelect",
        "OutSelect",
        "OutSelect",
        "AmtSelect",
        "CrcySelect",
        "LedgerSelect",
        "SQLselect",
        "Inputs",
        "Data2ExcelHdrs1",
        "Data2ExcelHdrs2",
        "Data2ExcelOrderBy",
        "Data2ExcelComma",
        "Data2ExcelEndColm",
        "Data2ExcelPivot",
        "Data2TabName",
    }.ToFrozenSet(StringComparer.OrdinalIgnoreCase);

    private readonly static IReadOnlyList<Func<string, MiscTablesRecord, bool>> _requiredTablePredicates = new Func<string, MiscTablesRecord, bool>[]
    {
        (id, subject) => _requiredTableIds.Contains(subject.Table_ID) && subject.ID.Equals(id, StringComparison.OrdinalIgnoreCase),
        (id, subject) => subject.Table_ID.Equals("SQL-" + id, StringComparison.OrdinalIgnoreCase),
        (id, subject) => subject.Table_ID.StartsWith("JE_Row_", StringComparison.OrdinalIgnoreCase) && subject.ID.Equals(id, StringComparison.OrdinalIgnoreCase),
    };

    public static bool IsRequiredTableId(string id, MiscTablesRecord subject)
    {
        return _requiredTablePredicates.Any(predicate => predicate(id, subject));
    }

    public static bool IsUserCoAccess(this UserAccessRecord subject) => subject.NotNull().Table_ID.Equals("UserCoAccess", StringComparison.OrdinalIgnoreCase);
}
