using System.Text.Json.Serialization;
using Crit.Domain.Identity;

namespace Crit.Domain.Entities;

public class OrganizationAdmin
{
	public OrganizationAdmin()
	{
		OrganizationId = Guid.Empty;
	}

	[JsonConstructor]
	public OrganizationAdmin(Guid organizationId, Guid adminUserId)
	{
		OrganizationId = organizationId;
		AdminUserId = adminUserId;
	}

	public Guid? Id { get; set; }
	public Guid OrganizationId { get; set; }
	public Guid AdminUserId { get; set; }

	[JsonIgnore]
	public virtual Organization? Organization { get; set; }
	[JsonIgnore]
	public virtual ApplicationUser? AdminUser { get; set; }

}
