
using Crit.Domain.Identity;
using Crit.Domain.Modules.OrganizationManagement.Entities;

namespace Crit.Domain.Entities;

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
