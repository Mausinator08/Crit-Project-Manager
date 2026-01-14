namespace Crit.Contracts.RequestModels;

public class CreateProjectRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? OwningOrganizationId { get; set; }
    public List<Guid> OrganizationIds { get; set; } = new List<Guid>();
    public List<Guid> ProjectUserIds { get; set; } = new List<Guid>();
    public List<Guid> ProjectAdminUserIds { get; set; } = new List<Guid>();
    public Guid? ProjectOwnerUserId { get; set; }
}
