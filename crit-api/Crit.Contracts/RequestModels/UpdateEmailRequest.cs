namespace Crit.Contracts.RequestModels;

public class UpdateEmailRequest
{
	public string? EmailAddress { get; set; }
	public Guid? OrganizationId { get; set; }
	public Guid? UserId { get; set; }
}
