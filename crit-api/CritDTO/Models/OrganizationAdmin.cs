using System.Text.Json.Serialization;
using CritDTO.Identity;

namespace CritDTO.Models;

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
