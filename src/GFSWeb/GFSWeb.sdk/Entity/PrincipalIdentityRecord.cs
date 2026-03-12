using System;
using System.Collections.Generic;
using System.Text;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Entity;

public record PrincipalIdentityRecord
{
    // PK
    public string NameIdentifier { get; init; } = null!;
    public string UserName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public bool Disabled { get; init; }

    public static IValidator<PrincipalIdentityRecord> Validator { get; } = new Validator<PrincipalIdentityRecord>()
        .RuleFor(x => x.NameIdentifier).NotEmpty()
        .RuleFor(x => x.UserName).NotEmpty()
        .RuleFor(x => x.Email).NotEmpty()
        .Build();
}


public static class PrincipalIdentityRecordExtensions
{
    public static Option<IValidatorResult> Validate(this PrincipalIdentityRecord record) => PrincipalIdentityRecord.Validator.Validate(record);
}