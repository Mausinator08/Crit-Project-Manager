namespace CritBusinessLogic.Models;

public class ProjectRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public Guid? OwningOrganizationId { get; set; } = Guid.Empty;
    public List<Guid> OrganizationIds { get; set; } = new List<Guid>();
    public List<Guid> ProjectUserIds { get; set; } = new List<Guid>();
    public List<Guid> ProjectAdminUserIds { get; set; } = new List<Guid>();
    public Guid? ProjectOwnerUserId { get; set; } = Guid.Empty;
}
