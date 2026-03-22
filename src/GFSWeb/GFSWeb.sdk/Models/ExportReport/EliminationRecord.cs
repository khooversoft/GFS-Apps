namespace GFSWeb.sdk.Models;

public record EliminationRecord
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
