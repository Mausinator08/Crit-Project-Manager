using Crit.Abstractions.Types;
using Crit.Contracts.ResponseModels;

namespace Crit.Contracts.RequestModels;

public class UpdateOrganizationRequest
{
	public string? Name { get; set; }
	public string? Email { get; set; }
	public string? CountryCode { get; set; }
	public string? PhoneNumber { get; set; }
	public PhoneNumberType? NumberType { get; set; }
	public string? Extension { get; set; }
	public Guid? OwnerUserId { get; set; }
	public List<Guid>? OrganizationAdminIds { get; set; }
	public List<Guid>? OrganizationMemberIds { get; set; }
	public List<Guid>? OrganizationAffiliateIds { get; set; }
	public List<Guid>? ProjectIds { get; set; }
}
