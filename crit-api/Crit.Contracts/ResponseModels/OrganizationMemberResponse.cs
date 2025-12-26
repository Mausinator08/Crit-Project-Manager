using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class OrganizationMemberResponse
{
	public OrganizationMemberResponse()
	{
		OrganizationId = Guid.Empty;
	}

	public Guid? Id { get; set; }
	public Guid OrganizationId { get; set; }
	public Guid MemberUserId { get; set; }
}
