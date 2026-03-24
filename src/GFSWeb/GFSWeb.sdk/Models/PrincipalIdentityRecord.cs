using System.Collections.Frozen;
using Toolbox.Extensions;
using Toolbox.Tools;
using Toolbox.Types;

namespace GFSWeb.sdk.Models;

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
    public string? UserID_SAP { get; set; }
    public string? FirstName { get; set; }
    public string? NickName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Location { get; set; }
    public string? ParkPost { get; set; }
    public string? PostEmail { get; set; }
    public string? CCEmail1 { get; set; }
    public string? CCEmail2 { get; set; }
    public string? Elim { get; set; }
    public string? Co_Update { get; set; }
    public string? Co_View { get; set; }
    public string? CC_NodeID { get; set; }
    public string? Flex1 { get; set; }
    public string? Flex2 { get; set; }
    public string? Flex3 { get; set; }
    public string? PostEmail2 { get; set; }

    private static string? Normalize(string? value) => value.ToNullIfEmpty();

    public virtual bool Equals(PrincipalIdentityRecord? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;

        return Normalize(NameIdentifier) == Normalize(other.NameIdentifier)
            && Normalize(UserName) == Normalize(other.UserName)
            && Normalize(Email) == Normalize(other.Email)
            && Disabled == other.Disabled
            && Normalize(Role) == Normalize(other.Role)
            && Parker == other.Parker
            && ParkerPost == other.ParkerPost
            && Normalize(UserID_SAP) == Normalize(other.UserID_SAP)
            && Normalize(FirstName) == Normalize(other.FirstName)
            && Normalize(NickName) == Normalize(other.NickName)
            && Normalize(MiddleName) == Normalize(other.MiddleName)
            && Normalize(LastName) == Normalize(other.LastName)
            && Normalize(Location) == Normalize(other.Location)
            && Normalize(ParkPost) == Normalize(other.ParkPost)
            && Normalize(PostEmail) == Normalize(other.PostEmail)
            && Normalize(CCEmail1) == Normalize(other.CCEmail1)
            && Normalize(CCEmail2) == Normalize(other.CCEmail2)
            && Normalize(Elim) == Normalize(other.Elim)
            && Normalize(Co_Update) == Normalize(other.Co_Update)
            && Normalize(Co_View) == Normalize(other.Co_View)
            && Normalize(CC_NodeID) == Normalize(other.CC_NodeID)
            && Normalize(Flex1) == Normalize(other.Flex1)
            && Normalize(Flex2) == Normalize(other.Flex2)
            && Normalize(Flex3) == Normalize(other.Flex3)
            && Normalize(PostEmail2) == Normalize(other.PostEmail2);
    }

    public override int GetHashCode()
    {
        var hash = new System.HashCode();

        hash.Add(Normalize(NameIdentifier));
        hash.Add(Normalize(UserName));
        hash.Add(Normalize(Email));
        hash.Add(Disabled);
        hash.Add(Normalize(Role));
        hash.Add(Parker);
        hash.Add(ParkerPost);
        hash.Add(Normalize(UserID_SAP));
        hash.Add(Normalize(FirstName));
        hash.Add(Normalize(NickName));
        hash.Add(Normalize(MiddleName));
        hash.Add(Normalize(LastName));
        hash.Add(Normalize(Location));
        hash.Add(Normalize(ParkPost));
        hash.Add(Normalize(PostEmail));
        hash.Add(Normalize(CCEmail1));
        hash.Add(Normalize(CCEmail2));
        hash.Add(Normalize(Elim));
        hash.Add(Normalize(Co_Update));
        hash.Add(Normalize(Co_View));
        hash.Add(Normalize(CC_NodeID));
        hash.Add(Normalize(Flex1));
        hash.Add(Normalize(Flex2));
        hash.Add(Normalize(Flex3));
        hash.Add(Normalize(PostEmail2));

        return hash.ToHashCode();
    }

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