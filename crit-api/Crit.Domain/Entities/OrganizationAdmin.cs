
using Crit.Domain.Identity;
using Crit.Domain.Modules.OrganizationManagement.Entities;

namespace Crit.Domain.Entities;

public class OrganizationAdmin
{
	public OrganizationAdmin()
	{
		OrganizationId = Guid.Empty;
	}

	public Guid? Id { get; set; }
	public Guid OrganizationId { get; set; }
	public Guid AdminUserId { get; set; }

	public Organization? Organization { get; set; }
	public ApplicationUser? AdminUser { get; set; }

}
