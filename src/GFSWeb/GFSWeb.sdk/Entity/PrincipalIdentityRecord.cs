using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Text;
using Toolbox.Extensions;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Entity;

public static class IdentityRole
{
    public const string ReaderRole = "reader";
    public const string ContributorRole = "contributor";
    public const string OwnerRole = "owner";

    public static FrozenSet<string> ValidRoles { get; } = new string[] { IdentityRole.ReaderRole, IdentityRole.ContributorRole, IdentityRole.OwnerRole }.ToFrozenSet();
}

public record PrincipalIdentityRecord
{
    // PK
    public string NameIdentifier { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool Disabled { get; set; }
    public string Role { get; set; } = null!;
    public bool Parker { get; set; }
    public bool ParkerPost { get; set; }

    public bool IsReader => Role == IdentityRole.ReaderRole;
    public bool IsContributor => Role == IdentityRole.ContributorRole;
    public bool IsOwner => Role == IdentityRole.OwnerRole;

    public bool Like(string? pattern) => NameIdentifier.Like(pattern) || UserName.Like(pattern) || Email.Like(pattern);

    public static IValidator<PrincipalIdentityRecord> Validator { get; } = new Validator<PrincipalIdentityRecord>()
        .RuleFor(x => x.NameIdentifier).NotEmpty()
        .RuleFor(x => x.UserName).NotEmpty()
        .RuleFor(x => x.Email).NotEmpty()
        .RuleFor(x => x.Role).NotEmpty().Must(x => IdentityRole.ValidRoles.Contains(x) ? StatusCode.OK : StatusCode.NotFound)
        .Build();
}


public static class PrincipalIdentityRecordExtensions
{
    public static Option<IValidatorResult> Validate(this PrincipalIdentityRecord record) => PrincipalIdentityRecord.Validator.Validate(record);
}