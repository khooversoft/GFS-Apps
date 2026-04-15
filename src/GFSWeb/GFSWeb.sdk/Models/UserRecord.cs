namespace GFSWeb.sdk.Models;

public record UserRecord
{
	public string UserID_Network { get; init; } = null!;
	public string? UserID_SAP { get; init; }
	public string? FirstName { get; init; }
	public string? NickName { get; init; }
	public string? MiddleName { get; init; }
	public string? LastName { get; init; }
	public string? Location { get; init; }
	public string? ParkPost { get; init; }
	public string? Email { get; init; }
	public string? PostEmail { get; init; }
	public string? CCEmail1 { get; init; }
	public string? CCEmail2 { get; init; }
	public string? Elim { get; init; }
	public string? Co_Update { get; init; }
	public string? Co_View { get; init; }
	public string? CC_NodeID { get; init; }
	public string? Flex1 { get; init; }
	public string? Flex2 { get; init; }
	public string? Flex3 { get; init; }
	public string? PostEmail2 { get; init; }
}
