using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Models;

public record ElimSqlCommand
{
    public string Table_ID { get; init; } = null!;
    public ElimCommandType? CommandType { get; init; }
    public string ID { get; init; } = null!;
    public string Descr { get; init; } = null!;

    public static IValidator<ElimSqlCommand> Validator { get; } = new Validator<ElimSqlCommand>()
        .RuleFor(x => x.Table_ID).NotEmpty()
        .RuleFor(x => x.CommandType).ValidateOption(ElimCommandType.Validator)
        .RuleFor(x => x.ID).NotEmpty()
        .RuleFor(x => x.Descr).NotEmpty()
        .Build();
}

public record ElimCommandType
{
    public string CommandType { get; init; } = null!;
    public int Id { get; init; }

    public static IValidator<ElimCommandType> Validator { get; } = new Validator<ElimCommandType>()
        .RuleFor(x => x.CommandType).NotEmpty()
        .RuleFor(x => x.Id).Must(x => x > 0 ? StatusCode.OK : StatusCode.BadRequest)
        .Build();
}

public static class ElimSqlCommandExtensions
{
    public static Option Validate(this ElimSqlCommand record) => ElimSqlCommand.Validator.Validate(record).ToOptionStatus();
}
