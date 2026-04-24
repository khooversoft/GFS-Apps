using Toolbox.Data;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk;

public record GfsWebOption
{
    public string AdminConnectionString { get; init; } = null!;
    public string VaultUri { get; init; } = null!;
    public ClientSecretOption Credentials { get; init; } = null!;
    public StoreOption UserStore { get; init; } = null!;
    public StoreOption ScheduleStore { get; init; } = null!;

    public static IValidator<GfsWebOption> Validator { get; } = new Validator<GfsWebOption>()
        .RuleFor(x => x.AdminConnectionString).NotEmpty()
        .RuleFor(x => x.VaultUri).NotEmpty()
        .RuleFor(x => x.UserStore).Validate(StoreOption.Validator)
        .RuleFor(x => x.ScheduleStore).Validate(StoreOption.Validator)
        .Build();
}

public record StoreOption
{
    public string Account { get; init; } = null!;
    public string Container { get; init; } = null!;
    public string BasePath { get; init; } = null!;

    public static IValidator<StoreOption> Validator { get; } = new Validator<StoreOption>()
        .RuleFor(x => x.Account).NotEmpty()
        .RuleFor(x => x.Container).NotEmpty()
        .RuleFor(x => x.BasePath).NotEmpty()
        .Build();
}


public static class GfsWebOptionExtensions
{
    public static Option<IValidatorResult> Validate(this GfsWebOption option) => GfsWebOption.Validator.Validate(option);
}