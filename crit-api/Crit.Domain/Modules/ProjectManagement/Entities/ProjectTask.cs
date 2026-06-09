

namespace Crit.Domain.Modules.ProjectManagement.Entities;

public class ProjectTask
{
    public ProjectTask()
    {
        ProjectId = Guid.Empty;
        SubProjectTasks = new List<ProjectTask>();
        CustomFields = new List<CustomField>();
        Comments = new List<Comment>();
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
    public Guid? ParentProjectTaskId { get; set; }

    public List<CustomField> CustomFields { get; set; }
    public Project? Project { get; set; }
    public Status? Status { get; set; }
    public Priority? Priority { get; set; }
    public List<ProjectTask> SubProjectTasks { get; set; }
    public ProjectTask? ParentProjectTask { get; set; }
    public List<Comment> Comments { get; set; }
}
