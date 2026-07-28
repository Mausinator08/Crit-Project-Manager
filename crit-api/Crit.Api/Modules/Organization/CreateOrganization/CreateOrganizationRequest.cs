using Crit.Abstractions.Types;

namespace CritApi.Modules.Organization.CreateOrganization;

public class CreateOrganizationRequest
{
	public string Name { get; set; } = string.Empty;
	public string? Email { get; set; } = string.Empty;
	public string? CountryCode { get; set; } = string.Empty;
	public string? PhoneNumber { get; set; } = string.Empty;
	public PhoneNumberType NumberType { get; set; } = PhoneNumberType.OrganizationPrimary;
	public string? Extension { get; set; }
	public Guid OwnerUserId { get; set; } = Guid.Empty;
	public List<Guid> OrganizationAdminIds { get; set; } = new List<Guid>();
	public List<Guid> OrganizationMemberIds { get; set; } = new List<Guid>();
	public List<Guid> OrganizationAffiliateIds { get; set; } = new List<Guid>();
}
