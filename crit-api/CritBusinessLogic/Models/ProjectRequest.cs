namespace CritBusinessLogic.Models;

public class ProjectRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string? OwningOrganizationId { get; set; } = string.Empty;
    public List<string> OrganizationIds { get; set; } = new List<string>();
    public List<string> ProjectUserIds { get; set; } = new List<string>();
    public List<string> ProjectAdminUserIds { get; set; } = new List<string>();
    public string? ProjectOwnerUserId { get; set; } = string.Empty;
}
