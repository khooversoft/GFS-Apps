using Toolbox.Extensions;
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
    public string Type { get; set; } = null!;
    public string Data { get; set; } = null!;
    public string Hash { get; set; } = null!;
    public bool Disabled { get; set; }

    public static IValidator<CommandRecord> Validator { get; } = new Validator<CommandRecord>()
        .RuleFor(x => x.CommandId).NotEmpty()
        .RuleFor(x => x.Description).NotEmpty()
        .RuleFor(x => x.Type).NotEmpty()
        .RuleFor(x => x.Data).NotEmpty()
        .RuleFor(x => x.Hash).NotEmpty()
        .RuleForObject(x => x).Must(x => x.Data.ToLowerInvariant().ToHashHex(true) == x.Hash, _ => "Hash not match")
        .Build();
}

public static class CommandRecordExtensions
{
    public static Option Validate(this CommandRecordPackage record) => CommandRecordPackage.Validator.Validate(record).ToOptionStatus();
    public static Option Validate(this CommandRecord record) => CommandRecord.Validator.Validate(record).ToOptionStatus();
}

public class CommandRecordBuilder
{
    public CommandRecordBuilder() { }

    public CommandRecordBuilder(CommandRecord record)
    {
        CommandId = record.CommandId;
        Description = record.Description;
        Type = record.Type;
        Data = record.Data;
    }

    public string? CommandId { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public string? Data { get; set; }

    public CommandRecordBuilder SetCommandId(string commandId) => this.Action(x => x.CommandId = commandId.NotEmpty());
    public CommandRecordBuilder SetDescription(string description) => this.Action(x => x.Description = description.NotEmpty());
    public CommandRecordBuilder SetType(string type) => this.Action(x => x.Type = type.NotEmpty());
    public CommandRecordBuilder SetData(string data) => this.Action(x => x.Data = data.NotEmpty());

    public CommandRecord Build() => new CommandRecord
    {
        CommandId = CommandId.NotEmpty(),
        Description = Description.NotEmpty(),
        Type = Type.NotEmpty(),
        Data = Data.NotEmpty(),
        Hash = Data.ToLowerInvariant().ToHashHex(true)
    };
}
