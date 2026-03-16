using System;
using System.Collections.Generic;
using System.Text;
using GFSWeb.sdk.Models;

namespace GFSWebTool.Stores;

internal sealed record EliminationImportRecord
{
    public int ID { get; init; }
    public string? ShortName { get; init; }
    public string? Def { get; init; }
    public string? ElimMapNbr { get; init; }
    public string? ElimMapName { get; init; }
    public string? ContractPrefix { get; init; }
    public string? OffsetBC { get; init; }
    public string? OffsetPC { get; init; }
    public string? OffsetPCdac { get; init; }
    public string? ForcePC { get; init; }
    public string? ForceTP { get; init; }
    public string? UWyr { get; init; }
    public string? StatLedger { get; init; }
    public string? Notes { get; init; }
    public string? TrueUpCo { get; init; }
    public string? TrueUpAorC { get; init; }
    public string? TrueUpMap { get; init; }
    public string? TrueUpPC { get; init; }
    public string? TrueUpBC { get; init; }
    public string? TrueUpTP { get; init; }
    public string? TrueUpContract { get; init; }
    public DateTime DateTimeStamp { get; init; }
}

internal static class EliminationImportRecordExtensions
{
    public static EliminationRecord ConvertTo(this EliminationImportRecord record) => new EliminationRecord
    {
        ID = record.ID,
        ShortName = record.ShortName,
        Def = record.Def,
        ElimMapNbr = record.ElimMapNbr,
        ElimMapName = record.ElimMapName,
        ContractPrefix = record.ContractPrefix,
        OffsetBC = record.OffsetBC,
        OffsetPC = record.OffsetPC,
        OffsetPCdac = record.OffsetPCdac,
        ForcePC = record.ForcePC,
        ForceTP = record.ForceTP,
        UWyr = record.UWyr,
        StatLedger = record.StatLedger,
        Notes = record.Notes,
        TrueUpCo = record.TrueUpCo,
        TrueUpAorC = record.TrueUpAorC,
        TrueUpMap = record.TrueUpMap,
        TrueUpPC = record.TrueUpPC,
        TrueUpBC = record.TrueUpBC,
        TrueUpTP = record.TrueUpTP,
        TrueUpContract = record.TrueUpContract
    };
}
