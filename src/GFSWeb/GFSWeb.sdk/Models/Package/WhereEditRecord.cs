namespace GFSWeb.sdk.Models;

public record WhereEditRecord
{
    public int PassNumber { get; set; }
    public IReadOnlyCollection<string> FieldNames { get; set; } = null!;
    public List<WhereClause> WhereItems { get; set; } = null!;
}
