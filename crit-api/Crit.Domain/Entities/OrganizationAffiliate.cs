
using Crit.Domain.Identity;
using Crit.Domain.Modules.OrganizationManagement.Entities;

namespace Crit.Domain.Entities;

public class OrganizationAffiliate
{
	public OrganizationAffiliate()
	{
		OrganizationId = Guid.Empty;
	}

	public Guid? Id { get; set; }
	public Guid OrganizationId { get; set; }
	public Guid AffiliateUserId { get; set; }

	public Organization? Organization { get; set; }
	public ApplicationUser? AffiliateUser { get; set; }

}
