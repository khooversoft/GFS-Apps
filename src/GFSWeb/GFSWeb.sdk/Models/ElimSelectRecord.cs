using System.Collections.Frozen;

namespace GFSWeb.sdk.Models;

public record ElimSelectRecord
{
    public string ElimID { get; init; } = null!;
    public int Pass { get; init; }
    public int SubSeq { get; init; }
    public string? FieldName { get; init; }
    public int FieldNbr { get; init; }
    public string? IncExcl { get; init; }
    public string? Oper { get; init; }
    public string? FromVal { get; init; }
    public string? ThruVal { get; init; }
    public string? GLSU { get; init; }
    public DateTime? DateTimeStamp { get; init; }
}


public static class ElimSelectRecordTool
{
    public static FrozenSet<string> FieldNames { get; } = new[]
    {
        "ProdCode",
        "BusCat",
        "CType",
        "Co",
        "Ledger",
        "TTY",
        "PC",
        "CC",
        "TP",
        "Hier",
        "GL",
        "TC",
        "SUBPRO",
        "Contract",
        "POSTDATE"
    }.ToFrozenSet(StringComparer.OrdinalIgnoreCase);
}
