using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Tools;

namespace GFSWeb.sdk.Models;

public class ElimReportRecord
{
    public string ShortName { get; init; } = null!;
    public string Description { get; init; } = null!;

    public EliminationRecord Elimination { get; init; } = null!;
    public IReadOnlyList<ElimSelectRecord> ElimSelects { get; init; } = Array.Empty<ElimSelectRecord>();
    public IReadOnlyList<ElimSqlCommand> SqlCommands { get; init; } = Array.Empty<ElimSqlCommand>();

    public static IValidator<ElimReportRecord> Validator { get; } = new Validator<ElimReportRecord>()
        .RuleFor(x => x.ShortName).NotEmpty()
        .RuleFor(x => x.Description).NotEmpty()
        .RuleFor(x => x.Elimination).NotNull()
        .RuleForEach(x => x.ElimSelects).Validate(ElimSelectRecord.Validator)
        .RuleFor(x => x.SqlCommands).NotNull()
        .RuleForEach(x => x.SqlCommands).Validate(ElimSqlCommand.Validator)
        .Build();
}
