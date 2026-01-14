using Crit.Abstractions.Types;

namespace Crit.Contracts.RequestModels;

public class CreatePhoneNumberRequest
{
	public string Number { get; set; } = string.Empty;
	public Guid OrganizationId { get; set; } = Guid.Empty;
	public string? CountryCode { get; set; }
	public string? Extension { get; set; }
	public PhoneNumberType Type { get; set; }
	public Guid UserId { get; set; }
}
