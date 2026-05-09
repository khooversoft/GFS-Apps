using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Models;

public record CommandRecordPackage
{
    public IReadOnlyList<CommandRecord> Commands { get; init; } = Array.Empty<CommandRecord>();

    public static IValidator<CommandRecordPackage> Validator { get; } = new Validator<CommandRecordPackage>()
        .RuleFor(x => x.Commands).NotNull()
        .RuleForEach(x => x.Commands).Validate(CommandRecord.Validator)
        .Build();
}

public record CommandRecord
{
    public string CommandId { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Data { get; set; } = null!;
    public bool Disabled { get; set; }

    public static IValidator<CommandRecord> Validator { get; } = new Validator<CommandRecord>()
        .RuleFor(x => x.CommandId).NotEmpty()
        .RuleFor(x => x.Description).NotEmpty()
        .RuleFor(x => x.Data).NotEmpty()
        .Build();
}

public static class CommandRecordExtensions
{
    public static Option Validate(this CommandRecordPackage record) => CommandRecordPackage.Validator.Validate(record).ToOptionStatus();
    public static Option Validate(this CommandRecord record) => CommandRecord.Validator.Validate(record).ToOptionStatus();
}
