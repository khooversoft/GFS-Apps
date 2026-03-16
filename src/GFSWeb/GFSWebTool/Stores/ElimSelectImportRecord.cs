using System;
using System.Collections.Generic;
using System.Text;
using GFSWeb.sdk.Models;

namespace GFSWebTool.Stores;

internal class ElimSelectImportRecord
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

internal static class ElimSelectImportRecordExtensions
{
    public static ElimSelectRecord ConvertTo(this ElimSelectImportRecord record) => new ElimSelectRecord
    {
        ElimID = record.ElimID,
        Pass = record.Pass,
        SubSeq = record.SubSeq,
        FieldName = record.FieldName ?? string.Empty,
        FieldNbr = record.FieldNbr,
        IncExcl = record.IncExcl,
        Oper = record.Oper,
        FromVal = record.FromVal,
        ThruVal = record.ThruVal,
        GLSU = record.GLSU
    };
}