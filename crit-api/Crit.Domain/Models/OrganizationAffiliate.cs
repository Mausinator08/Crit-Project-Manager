
using Crit.Domain.Identity;
namespace Crit.Domain.Models;

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
