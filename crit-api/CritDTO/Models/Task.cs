using System.Text.Json.Serialization;

namespace CritDTO.Models;

public class ProjectTask : AuditInformation
{
    public ProjectTask()
    {
        ProjectId = string.Empty;
        SubTasks = new List<ProjectTask>();
        CustomFields = new List<CustomField>();
        DateTime now = DateTime.Now;
        DateCreated = now;
        DateUpdated = now;
    }

    [JsonConstructor]
    public ProjectTask(string projectId, string createdByUserId)
    {
        ProjectId = projectId;
        SubTasks = new List<ProjectTask>();
        CustomFields = new List<CustomField>();
        DateTime now = DateTime.Now;
        DateCreated = now;
        DateUpdated = now;
        CreatedByUserId = createdByUserId;
        UpdatedByUserId = createdByUserId;
    }

    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
    public string ProjectId { get; set; }
    public string? AssignedUserId { get; set; }
    public string? StatusId { get; set; }
    public string? PriorityId { get; set; }
    public int? Complexity { get; set; }
    public DateTime? DueDate { get; set; }
    public string? ParentTaskId { get; set; }

    public List<CustomField> CustomFields { get; set; }
    public Project? Project { get; set; }
    public Status? Status { get; set; }
    public Priority? Priority { get; set; }
    public List<ProjectTask> SubTasks { get; set; }
    public ProjectTask? ParentTask { get; set; }
}
