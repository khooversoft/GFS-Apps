using System.Collections.Frozen;

namespace GFSWeb.sdk.Models;

public static class IdentityRole
{
    public const string ReaderRole = "reader";
    public const string ContributorRole = "contributor";
    public const string OwnerRole = "owner";

    public static FrozenSet<string> ValidRoles { get; } = new string[] { IdentityRole.ReaderRole, IdentityRole.ContributorRole, IdentityRole.OwnerRole }.ToFrozenSet();
}
