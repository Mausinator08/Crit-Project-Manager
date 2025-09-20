using System.Text.Json.Serialization;
using CritDTO.Identity;

namespace CritDTO.Models;

public class OrganizationAffiliate
{
	public OrganizationAffiliate()
	{
		OrganizationId = Guid.Empty;
	}

	[JsonConstructor]
	public OrganizationAffiliate(Guid organizationId, Guid affiliateUserId)
	{
		OrganizationId = organizationId;
		AffiliateUserId = affiliateUserId;
	}

	public Guid? Id { get; set; }
	public Guid OrganizationId { get; set; }
	public Guid AffiliateUserId { get; set; }

	[JsonIgnore]
	public virtual Organization? Organization { get; set; }
	[JsonIgnore]
	public virtual ApplicationUser? AffiliateUser { get; set; }

}
