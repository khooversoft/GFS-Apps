using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.Application;

public record AppRegistrationOption
{
    public string VaultUri { get; init; } = null!;
    public string TenantId { get; init; } = null!;
    public string ClientId { get; init; } = null!;
    public string ClientSecret { get; init; } = null!;

    public StoreOption Store { get; init; } = null!;

    public static IValidator<AppRegistrationOption> Validator { get; } = new Validator<AppRegistrationOption>()
        .RuleFor(x => x.VaultUri).NotEmpty()
        .RuleFor(x => x.TenantId).NotEmpty()
        .RuleFor(x => x.ClientId).NotEmpty()
        .RuleFor(x => x.Store).Validate(StoreOption.Validator)
        .Build();
}

public record StoreOption
{
    public string UserReport { get; init; } = null!;
    public string ScheduleReport { get; init; } = null!;
    public string BasePath { get; init; } = null!;

    public static IValidator<StoreOption> Validator { get; } = new Validator<StoreOption>()
        .RuleFor(x => x.UserReport).NotEmpty()
        .RuleFor(x => x.ScheduleReport).NotEmpty()
        .RuleFor(x => x.BasePath).NotEmpty()
        .Build();
}


public static class AppRegistrationOptionExtensions
{
    public static Option<IValidatorResult> Validate(this AppRegistrationOption option) => AppRegistrationOption.Validator.Validate(option);
}

