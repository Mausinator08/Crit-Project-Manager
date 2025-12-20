using System.Text.Json.Serialization;
using Crit.Domain.Identity;

namespace Crit.Domain.Entities;

public class OrganizationMember
{
	public OrganizationMember()
	{
		OrganizationId = Guid.Empty;
	}

	[JsonConstructor]
	public OrganizationMember(Guid organizationId, Guid memberUserId)
	{
		OrganizationId = organizationId;
		MemberUserId = memberUserId;
	}

	public Guid? Id { get; set; }
	public Guid OrganizationId { get; set; }
	public Guid MemberUserId { get; set; }

	[JsonIgnore]
	public virtual Organization? Organization { get; set; }
	[JsonIgnore]
	public virtual ApplicationUser? MemberUser { get; set; }

}
