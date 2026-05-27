namespace GFSWeb.sdk.Models;

public enum SectionId
{
    ReconExcel,
    Sql,
    Pivot,
    Je,
    FieldSelect,
    OutSelect,
    Data2Excel,
    Parameters
}

public record MiscTablesRecord
{
    public string Table_ID { get; set; } = null!;
    public string ID { get; set; } = null!;
    public string Descr { get; set; } = null!;
    public string? Field1 { get; set; }
    public string? Field2 { get; set; }
    public string? Field3 { get; set; }
    public string? Field4 { get; set; }
    public string? Field5 { get; set; }
    public string? Field6 { get; set; }
}


public static class MiscTablesRecordTool
{
    public static SectionId GetSectionId(this MiscTablesRecord subject) => subject.Table_ID.ToLower() switch
    {
        string s when s.StartsWith("reconexcel") => SectionId.ReconExcel,
        string s when s.StartsWith("sql-") => SectionId.Sql,
        string s when s.StartsWith("pivot1") => SectionId.Pivot,
        string s when s.StartsWith("je_row") => SectionId.Je,
        "fieldselect" => SectionId.FieldSelect,
        "outselect" => SectionId.OutSelect,
        string s when s.StartsWith("data2excel") => SectionId.Data2Excel,

        _ => SectionId.Parameters,
    };

    public static string GetTitle(this SectionId subject) => subject switch
    {
        SectionId.ReconExcel => "Recon Excel",
        SectionId.Sql => "SQL",
        SectionId.Pivot => "Pivot",
        SectionId.Je => "JE",
        SectionId.FieldSelect => "Field Select",
        SectionId.OutSelect => "Out Select",
        SectionId.Data2Excel => "Data to Excel",
        SectionId.Parameters => "Parameters",
        _ => throw new ArgumentException("Invalid SectionId", nameof(subject)),
    };
}