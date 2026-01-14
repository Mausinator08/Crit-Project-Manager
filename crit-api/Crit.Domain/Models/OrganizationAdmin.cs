
using Crit.Domain.Identity;
namespace Crit.Domain.Models;

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
