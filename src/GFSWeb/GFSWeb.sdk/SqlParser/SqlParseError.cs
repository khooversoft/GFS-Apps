using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace GFSWeb.sdk.SqlParser;

public record SqlParseError(string Message, int Number, int Offset, int Line, int Column)
{
}

public static class SqlParseErrorTool
{
    public static SqlParseError ConvertTo(this ParseError error)
    {
        return new SqlParseError(error.Message, error.Number, error.Offset, error.Line, error.Column);
    }
}
