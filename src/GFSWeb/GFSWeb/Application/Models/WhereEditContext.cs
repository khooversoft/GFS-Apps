namespace GFSWeb.Application.Models;

public class WhereEditContext
{
    public IReadOnlyList<string> FieldNames { get; set; } = null!;
    public List<WhereClause> WhereItems { get; set; } = null!;
    public IReadOnlyList<WhereClause> ReadWhereItems { get; set; } = null!;
}
