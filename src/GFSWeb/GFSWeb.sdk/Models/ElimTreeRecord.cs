using Toolbox.Extensions;

namespace GFSWeb.sdk.Models;

public record ElimTreeRecord
{
    public string SortKey { get; init; } = null!;
    public string Id { get; init; } = null!;
    public string Descr { get; init; } = null!;
    public string Parent { get; init; } = null!;
    public int? ElimId { get; init; }
    public string? PackageType { get; init; }
    public string? ShortName { get; init; }
    public string? Def { get; init; }
}

public static class ElimTreeRecordTool
{
    public static ElimTreeRecord ParseDetails(this ElimTreeRecord record)
    {
        record.Descr.IsNotEmpty();

        var elimIdAndType = ExtractElimIdAndType(record);
        var shortNameAndDef = ExtractShortNameAndDef(record);

        if (elimIdAndType == null && shortNameAndDef == null) return record;

        var newRecord = record with
        {
            ElimId = elimIdAndType?.elimId,
            PackageType = elimIdAndType?.elimType,
            ShortName = shortNameAndDef?.shortName,
            Def = shortNameAndDef?.def
        };

        return newRecord;
    }

    // Parse 'E03' from the Id and determine that 'E' is the elim type and '03' is the elim id
    private static (string elimType, int elimId)? ExtractElimIdAndType(ElimTreeRecord record) => record.Id switch
    {
        { Length: 3 } => (char.IsLetter(record.Id[0]), int.TryParse(record.Id[1..], out int value)) switch
        {
            (true, true) => (elimType: record.Id[0].ToString(), elimId: value),
            _ => null
        },
        _ => null,
    };

    // Format: "E03 - LatAm Synd (2007 ceding to 3030)" where "LatAm Synd" is the short name and "2007 ceding to 3030" is the def
    private static (string shortName, string def)? ExtractShortNameAndDef(ElimTreeRecord record)
    {
        const string marker = "-";

        int markerIndex = record.Descr.IndexOf(marker, StringComparison.Ordinal);
        if (markerIndex < 0) return null;

        int shortNameStart = markerIndex + marker.Length;
        int openParenIndex = record.Descr.IndexOf('(', shortNameStart);
        int closeParenIndex = record.Descr.LastIndexOf(')');

        if (openParenIndex < 0 || closeParenIndex <= openParenIndex) return null;

        string shortName = record.Descr[shortNameStart..openParenIndex].Trim();
        string def = record.Descr[(openParenIndex + 1)..closeParenIndex].Trim();

        if (shortName.Length == 0 && def.Length == 0) return null;
        return (shortName, def);
    }
}