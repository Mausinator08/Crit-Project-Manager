namespace Crit.Contracts.ResponseModels;

public class EmailResponse
{
    public Guid? Id { get; set; }
    public Guid? UserId { get; set; }
    public string? EmailAddress { get; set; }
    public Guid OrganizationId { get; set; } = Guid.Empty;
}
