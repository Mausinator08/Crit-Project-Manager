using Crit.Abstractions.Types;

namespace Crit.Contracts.RequestModels;

public class UpdatePhoneNumberRequest
{
	public string? Number { get; set; }
	public Guid? OrganizationId { get; set; }
	public string? CountryCode { get; set; }
	public string? Extension { get; set; }
	public PhoneNumberType? Type { get; set; }
	public Guid? UserId { get; set; }
}
