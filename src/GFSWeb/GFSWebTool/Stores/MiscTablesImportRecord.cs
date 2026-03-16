using Toolbox.Tools;
using Toolbox.Types;
using GFSWeb.sdk.Models;

namespace GFSWebTool.Stores;

internal sealed record MiscTablesImportRecord
{
    public string Table_ID { get; init; } = null!;
    public string ID { get; init; } = null!;
    public string Descr { get; init; } = null!;

    public ElimCommandType? GetCommandType() => Table_ID switch
    {
        string v when v.StartsWith("SQL-") && char.IsLetter(v[4]) && int.TryParse(v[5..], out var num) => new ElimCommandType { CommandType = v[4..5], Id = num },
        _ => null,
    };
}

internal static class MiscTablesImportRecordExtensions
{
    public static ElimSqlCommand ConvertTo(this MiscTablesImportRecord record)
    {
        ElimCommandType? commandType = record.GetCommandType();

        return new ElimSqlCommand
        {
            Table_ID = record.Table_ID,
            CommandType = commandType,
            ID = record.ID,
            Descr = record.Descr,
        };
    }
}