using GFSWeb.sdk.SqlParser;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Models;

public record SqlCommandActivity : IPackageActivity
{
    public string Id { get; set; } = null!;
    public string Type { get; } = "SQL Command";
    public string Description { get; set; } = null!;
    public string SqlCommand { get; set; } = null!;
    public string Hash { get; set; } = null!;

    public static IValidator<SqlCommandActivity> Validator { get; } = new Validator<SqlCommandActivity>()
        .RuleFor(x => x.Id).NotEmpty()
        .RuleFor(x => x.Description).NotEmpty()
        .RuleFor(x => x.SqlCommand).NotEmpty()
        .RuleFor(x => x.Hash).NotEmpty()
        .Build();
}

public static class SqlCommandActivityTool
{
    public static Option Validate(this SqlCommandActivity subject) => SqlCommandActivity.Validator.Validate(subject).ToOptionStatus();

    public static string GetCommandHash(this SqlCommandActivity subject) => SqlParserTool.GetSqlCommandHash(subject.SqlCommand);

    public static SqlCommandActivity WithHash(this SqlCommandActivity subject)
    {
        subject.NotNull().Description.NotEmpty();
        return subject with { Hash = subject.GetCommandHash() };
    }
}
