using System.Text.Json.Serialization;
using Crit.Domain.BaseModels;

namespace Crit.Domain.Entities;

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

    [JsonConstructor]
    public ProjectTask(Guid projectId, Guid createdByUserId)
    {
        ProjectId = projectId;
        SubTasks = new List<ProjectTask>();
        CustomFields = new List<CustomField>();
        Comments = new List<Comment>();
        DateTime now = DateTime.UtcNow;
        DateCreated = now;
        DateUpdated = now;
        CreatedByUserId = createdByUserId;
        UpdatedByUserId = createdByUserId;
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
    public virtual List<CustomField> CustomFields { get; set; }
    [JsonIgnore]
    public virtual Project? Project { get; set; }
    [JsonIgnore]
    public virtual Status? Status { get; set; }
    [JsonIgnore]
    public virtual Priority? Priority { get; set; }
    [JsonIgnore]
    public virtual List<ProjectTask> SubTasks { get; set; }
    [JsonIgnore]
    public virtual ProjectTask? ParentTask { get; set; }
    [JsonIgnore]
    public virtual List<Comment> Comments { get; set; }
}
