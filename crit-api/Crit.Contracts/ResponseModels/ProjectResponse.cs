using System.Text.Json.Serialization;

namespace Crit.Contracts.ResponseModels;

public class ProjectResponse : AuditInformationResponse
{
    public ProjectResponse()
    {
        Name = "";
        Tasks = new List<ProjectTaskResponse>();
        OwningOrganizationId = Guid.Empty;
        ProjectOwnerUserId = Guid.Empty;
        CustomFieldTypes = new List<CustomFieldTypeResponse>();
        Statuses = new List<StatusResponse>();
        Priorities = new List<PriorityResponse>();
        HiddenCustomFieldTypes = new List<CustomFieldTypeResponse>();
        Comments = new List<CommentResponse>();
        ProjectUsers = new List<ProjectUserResponse>();
        ProjectAdmins = new List<ProjectAdminResponse>();
        DateTime now = DateTime.UtcNow;
        DateCreated = now;
        DateUpdated = now;
    }

    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid OwningOrganizationId { get; set; }
    public Guid ProjectOwnerUserId { get; set; }

    public List<StatusResponse> Statuses { get; set; }
    public List<PriorityResponse> Priorities { get; set; }
    public List<CustomFieldTypeResponse> CustomFieldTypes { get; set; }
    public List<ProjectTaskResponse> Tasks { get; set; }
    public List<CustomFieldTypeResponse> HiddenCustomFieldTypes { get; set; }
    public List<CommentResponse> Comments { get; set; }
    public List<ProjectUserResponse> ProjectUsers { get; set; }
    public List<ProjectAdminResponse> ProjectAdmins { get; set; }
}
