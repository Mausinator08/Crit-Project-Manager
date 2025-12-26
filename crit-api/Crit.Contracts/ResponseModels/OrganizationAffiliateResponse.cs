using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class OrganizationAffiliateResponse
{
	public OrganizationAffiliateResponse()
	{
		OrganizationId = Guid.Empty;
	}

	public Guid? Id { get; set; }
	public Guid OrganizationId { get; set; }
	public Guid AffiliateUserId { get; set; }
}
