using System.Text.Json.Serialization;
using Crit.Domain.Identity;

namespace Crit.Domain.Models;

public class OrganizationMember
{
	public OrganizationMember()
	{
		OrganizationId = Guid.Empty;
	}

	public Guid? Id { get; set; }
	public Guid OrganizationId { get; set; }
	public Guid MemberUserId { get; set; }

	public Organization? Organization { get; set; }
	public ApplicationUser? MemberUser { get; set; }

}
