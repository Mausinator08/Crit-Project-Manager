using System.Text.Json.Serialization;

namespace CritDTO.Models;

public class Task : AuditInformation
{
    public Task()
    {
        ColaboratorUserIds = new List<Guid>();
        SubTasks = new List<Task>();
        CustomFields = new List<CustomField>();
        DateTime now = DateTime.Now;
        DateCreated = now;
        DateUpdated = now;
    }

    public Task(Guid projectId, Guid createdByUserId)
    {
        ProjectId = projectId;
        ColaboratorUserIds = new List<Guid>([createdByUserId]);
        SubTasks = new List<Task>();
        CustomFields = new List<CustomField>();
        DateTime now = DateTime.Now;
        DateCreated = now;
        DateUpdated = now;
        CreatedByUserId = createdByUserId;
        UpdatedByUserId = createdByUserId;
    }

    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
    public Guid ProjectId { get; set; }
    public List<Guid> ColaboratorUserIds { get; set; }
    public Guid? AssignedUserId { get; set; }
    public Guid? StatusId { get; set; }
    public Guid? PriorityId { get; set; }
    public int? Complexity { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? ParentTaskId { get; set; }

    public List<CustomField> CustomFields { get; set; }
    public Project? Project { get; set; }
    public Status? Status { get; set; }
    public Priority? Priority { get; set; }
    public List<Task> SubTasks { get; set; }
    public Task? ParentTask { get; set; }
}
