using System.Text.Json.Serialization;
using Crit.Domain.BaseModels;

namespace Crit.Domain.Models;

public class ProjectTask : AuditInformation
{
    public ProjectTask()
    {
        ProjectId = Guid.Empty;
        SubTasks = new List<ProjectTask>();
        CustomFields = new List<CustomField>();
        Comments = new List<Comment>();
        DateTime now = DateTime.UtcNow;
        DateCreated = now;
        DateUpdated = now;
    }

    public Guid? Id { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? AssignedUserId { get; set; }
    public Guid? StatusId { get; set; }
    public Guid? PriorityId { get; set; }
    public int? Complexity { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? ParentTaskId { get; set; }

    [JsonIgnore]
    public List<CustomField> CustomFields { get; set; }
    [JsonIgnore]
    public Project? Project { get; set; }
    [JsonIgnore]
    public Status? Status { get; set; }
    [JsonIgnore]
    public Priority? Priority { get; set; }
    [JsonIgnore]
    public List<ProjectTask> SubTasks { get; set; }
    [JsonIgnore]
    public ProjectTask? ParentTask { get; set; }
    [JsonIgnore]
    public List<Comment> Comments { get; set; }
}
