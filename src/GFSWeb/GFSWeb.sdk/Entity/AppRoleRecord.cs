using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Entity;

public record AppRoleRecord
{
    public string RoleCode { get; init; } = null!;
    public string Description { get; init; } = null!;

    public static IValidator<AppRoleRecord> Validator { get; } = new Validator<AppRoleRecord>()
        .RuleFor(x => x.RoleCode).NotEmpty()
        .RuleFor(x => x.Description).NotEmpty()
        .Build();
}

public static class AppRoleRecordExtensions
{
    public static Option<IValidatorResult> Validate(this AppRoleRecord record) => AppRoleRecord.Validator.Validate(record);
}

public static class PrincipalRoleExtensions
{
    public static Option<IValidatorResult> Validate(this PrincipalRole record) => PrincipalRole.Validator.Validate(record);
}