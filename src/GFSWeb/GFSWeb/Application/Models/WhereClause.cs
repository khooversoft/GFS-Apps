namespace GFSWeb.Application.Models;

public enum AndOr
{
    And,
    Or,
};

public enum Operator
{
    Equal,
    NotEqual,
    Between,
    NotBetween,
    ComparePattern,
    NotComparePattern,
}

public record WhereClause
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public AndOr AndOr { get; set; } = AndOr.And;
    public string? FieldName { get; set; }
    public Operator Operator { get; set; } = Operator.Equal;
    public string? Value { get; set; }
    public string? Value2 { get; set; }
}

public static class OperatorExtensions
{
    public static string ToSymbol(this Operator op)
    {
        return op switch
        {
            Operator.Equal => "=",
            Operator.NotEqual => "<>",
            Operator.Between => "between",
            Operator.NotBetween => "not between",
            Operator.ComparePattern => "pattern",
            Operator.NotComparePattern => "not pattern",
            _ => throw new ArgumentOutOfRangeException(nameof(op), op, null)
        };
    }

    public static bool IsValue2Required(this WhereClause where) => where.Operator is Operator.Between or Operator.NotBetween;
}