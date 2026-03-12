using Toolbox.Tools;

namespace GFSWeb.sdk.Entity;

public record PrincipalRole
{
    public string NameIdentifier { get; init; } = null!;
    public string RoleCode { get; init; } = null!;

    public static IValidator<PrincipalRole> Validator { get; } = new Validator<PrincipalRole>()
        .RuleFor(x => x.RoleCode).NotEmpty()
        .RuleFor(x => x.RoleCode).NotEmpty()
        .Build();
}
