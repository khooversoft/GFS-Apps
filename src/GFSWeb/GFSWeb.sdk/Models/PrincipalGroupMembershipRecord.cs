namespace GFSWeb.sdk.Models;

public record PrincipalGroupMembershipRecord
{
    public PrincipalGroupRecord Group { get; init; } = null!;
    public PrincipalIdentityRecord Identity { get; init; } = null!;
}
