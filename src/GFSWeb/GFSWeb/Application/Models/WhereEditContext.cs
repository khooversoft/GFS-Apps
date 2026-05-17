namespace GFSWeb.Application.Models;

public class WhereEditContext
{
    public int Pass { get; set; }
    public IReadOnlyCollection<string> FieldNames { get; set; } = null!;
    public List<WhereClause> WhereItems { get; set; } = null!;
    //public IReadOnlyList<WhereClause> ReadWhereItems { get; set; } = null!;
}
