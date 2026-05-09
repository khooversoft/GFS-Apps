using System;
using System.Collections.Generic;
using System.Text;
using GFSWeb.sdk.Models;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Toolbox.Extensions;
using Toolbox.Tools;

namespace GFSWeb.sdk.SqlParser;

public static class SqlParserTool
{
    public static CommandRecord? GenerateCommand(string sql)
    {
        var result = FormatLine(sql);
        if (result.Errors.Count > 0) return null;

        string formattedSql = result.formattedSql.ToLowerInvariant();
        string hash = formattedSql.ToHashHex(useMD5: true);

        var commandRecord = new CommandRecord
        {
            CommandId = hash,
            Description = $"<dummy>",
            Data = formattedSql,
            Disabled = false
        };

        return commandRecord;
    }

    public static (string formattedSql, IList<SqlParseError> Errors) FormatLine(string sql)
    {
        var parser = new TSql150Parser(false);
        var fragment = parser.Parse(new StringReader(sql), out var errors);

        if (errors.Count > 0)
        {
            return (sql, errors.Select(x => x.ConvertTo()).ToArray());
        }

        var generator = new Sql150ScriptGenerator(
            new SqlScriptGeneratorOptions
            {
                KeywordCasing = KeywordCasing.Uppercase,
                IncludeSemicolons = true,
                NewLineBeforeFromClause = false,
                NewLineBeforeWhereClause = false,
                NewLineBeforeGroupByClause = false,
                NewLineBeforeOrderByClause = false,
                NewLineBeforeJoinClause = false,
                NewLineBeforeOutputClause = false,
                NewLineBeforeHavingClause = false,
                IndentationSize = 0,
                AlignClauseBodies = false,
                MultilineSelectElementsList = false,
                MultilineWherePredicatesList = false,
                MultilineInsertSourcesList = false,
                MultilineInsertTargetsList = false,
                MultilineSetClauseItems = false,
                MultilineViewColumnsList = false,
            });

        generator.GenerateScript(fragment, out var rawSql);
        var formattedSql = System.Text.RegularExpressions.Regex.Replace(rawSql, @"\s+", " ").Trim();
        return (formattedSql, []);
    }

    public static (string FormattedSql, IList<SqlParseError> Errors) Format(string sql)
    {
        var parser = new TSql150Parser(false);
        var fragment = parser.Parse(new StringReader(sql), out var errors);

        if (errors.Count > 0)
        {
            return (sql, errors.Select(x => x.ConvertTo()).ToArray());
        }

        var generator = new Sql150ScriptGenerator(
            new SqlScriptGeneratorOptions
            {
                KeywordCasing = KeywordCasing.Uppercase,
                IndentationSize = 4,
                IndentSetClause = true,
                IndentViewBody = true,
                AlignClauseBodies = true,
                MultilineInsertSourcesList = true,
                MultilineInsertTargetsList = true,
                MultilineSelectElementsList = true,
                MultilineSetClauseItems = true,
                MultilineViewColumnsList = true,
                MultilineWherePredicatesList = true,
                IncludeSemicolons = true,
                NewLineBeforeFromClause = true,
                NewLineBeforeWhereClause = true,
                NewLineBeforeGroupByClause = true,
                NewLineBeforeOrderByClause = true,
            });

        generator.GenerateScript(fragment, out var formattedSql);
        return (formattedSql, []);
    }
}


