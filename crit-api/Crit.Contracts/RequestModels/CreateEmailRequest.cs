namespace Crit.Contracts.Emails;

public class CreateEmailRequest
{
	public string EmailAddress { get; set; } = string.Empty;
	public Guid OrganizationId { get; set; } = Guid.Empty;
	public Guid UserId { get; set; } = Guid.Empty;
}
